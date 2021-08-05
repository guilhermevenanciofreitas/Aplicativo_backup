using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using DFe.Classes.Flags;
using DFe.Classes.Extensoes;
using Entidades = DFe.Classes.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Classes = NFe.Classes;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Cobranca;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Transporte;

using NFe.Utils;
using NFe.Utils.NFe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using UAParser;
using System.IO;
using System.Reflection;

using NFe.Classes;
using NFe.Classes.Servicos.Tipos;
using NFe.Classes.Servicos.ConsultaCadastro;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using System.Threading;
using Aplicativo.View.Helpers;
using Build.Server.Controllers;

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {

        [HttpPost]
        [Route("[action]")]
        public Response Enviar([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                using var db = new Context();

                var EmpresaID = Request.GetParameter("EmpresaID").ToIntOrNull();

                var Empresa = db.Empresa
                    .Include(c => c.EmpresaCertificado).ThenInclude(c => c.Certificado).ThenInclude(c => c.Arquivo)
                    .FirstOrDefault(c => c.EmpresaID == EmpresaID);

                var Certificado = Empresa.EmpresaCertificado.FirstOrDefault().Certificado;

                var X509Certificate2 = new X509Certificate2(Certificado.Arquivo.Anexo, Certificado.Senha);

                var ConfiguracaoServico = new ConfiguracaoServico();

                ConfiguracaoServico.DiretorioSchemas = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Schemas");
                
                var ListNFe = new List<Classes.NFe>();

                foreach (var Item in JsonConvert.DeserializeObject<List<NotaFiscal>>(Request.GetParameter("NotaFiscal")))
                {

                    var NotaFiscal = db.NotaFiscal
                        .Include("NotaFiscalItem")
                        .FirstOrDefault(c => c.nNF == Item.nNF && c.serie == Item.serie);

                    var GerarNFe = new GerarNFe();

                    var NFe = GerarNFe.GetNf(NotaFiscal);

                    NFe = NFe.Assina(ConfiguracaoServico, X509Certificate2);

                    ListNFe.Add(NFe);

                }


                var ListNFeProc = HelpNFe.NFeAutorizacao(ConfiguracaoServico, X509Certificate2, ListNFe);

                return Response;

            }
            catch (Exception ex)
            {
                return _Exception.Response(ex, Response);
            }
        }



    }

    public class GerarNFe
    {

        private VersaoServico VersaoServico;
        private NotaFiscal NotaFiscal;


        public Classes.NFe GetNf(NotaFiscal NotaFiscal)
        {

            this.VersaoServico = VersaoServico.Versao400;

            this.NotaFiscal = NotaFiscal;

            var NFe = new Classes.NFe { infNFe = GetInf() };
            return NFe;
        }

        private infNFe GetInf()
        {

            var infNFe = new infNFe
            {
                versao = VersaoServico.VersaoServicoParaString(),
                ide = GetIdentificacao(),
                emit = GetEmitente(),
                dest = GetDestinatario(),
                transp = GetTransporte(),
            };

            var i = 0;
            foreach (var item in NotaFiscal.NotaFiscalItem)
            {
                i++;
                infNFe.det.Add(GetDetalhe(i, item));
            }


            infNFe.total = GetTotal(infNFe.det);

            //if (infNFe.ide.mod == ModeloDocumento.NFe)
            //{
            //    infNFe.cobr = GetCobranca(infNFe.total.ICMSTot);
            //}

            //if (infNFe.ide.mod == ModeloDocumento.NFCe)
            //{
            infNFe.pag = GetPagamento(infNFe.total.ICMSTot);
            //}

            return infNFe;

        }

        private ide GetIdentificacao()
        {

            var ide = new ide()
            {
                natOp = NotaFiscal.natOp,
                mod = (ModeloDocumento)Convert.ToInt32(NotaFiscal.mod),
                serie = (int)NotaFiscal.serie,
                nNF = (long)NotaFiscal.nNF,
                dhEmi = (DateTimeOffset)NotaFiscal.dhEmi,
                dhSaiEnt = NotaFiscal.dhSaiEnt,
                tpNF = (TipoNFe)NotaFiscal.tpNF,
                idDest = NotaFiscal.UF == NotaFiscal.dest_UF ? DestinoOperacao.doInterna : DestinoOperacao.doInterestadual,
                cMunFG = (long)NotaFiscal.cMun,
                tpEmis = (TipoEmissao)NotaFiscal.tpEmis,
                tpImp = TipoImpressao.tiRetrato,
                cNF = NotaFiscal.cNF ?? new Random().Next(1, 99999999).ToStringOrNull(),
                tpAmb = (TipoAmbiente)NotaFiscal.tpAmb,
                finNFe = (FinalidadeNFe)NotaFiscal.finNFe,
                cUF = new DFe.Classes.Entidades.Estado().SiglaParaEstado(NotaFiscal.UF.ToString()),
                procEmi = ProcessoEmissao.peAplicativoContribuinte,
                indFinal = ConsumidorFinal.cfNao,
                indPres = (PresencaComprador)NotaFiscal.indPres,
                verProc = "3.000",
            };

            if (string.IsNullOrEmpty(NotaFiscal.dest_IE))
            {
                ide.indFinal = ConsumidorFinal.cfConsumidorFinal;
            }

            if (ide.tpEmis != TipoEmissao.teNormal)
            {
                ide.dhCont = DateTime.Now;
                ide.xJust = "TESTE DE CONTIGÊNCIA PARA NFe/NFCe";
            }

            return ide;

        }

        private emit GetEmitente()
        {
            var emit = new emit
            {
                CNPJ = NotaFiscal.CNPJCPF,
                xNome = NotaFiscal.xNome,
                xFant = NotaFiscal.xFant,
                IE = NotaFiscal.IE,
                CRT = (CRT)NotaFiscal.CRT,
                enderEmit = GetEnderecoEmitente(),
            };

            return emit;
        }

        private enderEmit GetEnderecoEmitente()
        {
            var enderEmit = new enderEmit
            {
                xLgr = NotaFiscal.xLgr,
                nro = NotaFiscal.nro,
                xCpl = NotaFiscal.xCpl,
                xBairro = NotaFiscal.xBairro,
                cMun = (long)NotaFiscal.cMun,
                xMun = NotaFiscal.xMun,
                UF = new DFe.Classes.Entidades.Estado().SiglaParaEstado(NotaFiscal.UF),
                CEP = NotaFiscal.CEP,
                fone = Convert.ToInt64(NotaFiscal.Fone),
                cPais = 1058,
                xPais = "BRASIL",
            };

            return enderEmit;

        }

        private dest GetDestinatario()
        {

            var dest = new dest(VersaoServico)
            {
                CNPJ = NotaFiscal.dest_CNPJCPF,
                xNome = (TipoAmbiente)NotaFiscal.tpAmb == TipoAmbiente.Producao ? NotaFiscal.dest_xNome : "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL", //Obrigatório para NFe e opcional para NFCe
                enderDest = GetEnderecoDestinatario(), //Obrigatório para NFe e opcional para NFCe
                indIEDest = indIEDest.ContribuinteICMS,
                IE = NotaFiscal.dest_IE,
                email = "teste@gmail.com",
            };


            if (NotaFiscal.dest_IE == "ISENTO")
            {
                dest.indIEDest = indIEDest.Isento;
            }


            if (string.IsNullOrEmpty(NotaFiscal.dest_IE))
            {
                dest.indIEDest = indIEDest.NaoContribuinte;
            }


            return dest;

        }

        private enderDest GetEnderecoDestinatario()
        {

            var enderDest = new enderDest
            {
                xLgr = NotaFiscal.dest_xLgr,
                nro = NotaFiscal.dest_nro,
                xBairro = NotaFiscal.dest_xBairro,
                cMun = (long)NotaFiscal.dest_cMun,
                xMun = NotaFiscal.dest_xMun,
                UF = NotaFiscal.dest_UF,
                CEP = NotaFiscal.dest_CEP,
                cPais = 1058,
                xPais = "BRASIL",
            };

            return enderDest;
        }

        private det GetDetalhe(int i, NotaFiscalItem NotaFiscalItem)
        {
            var det = new det
            {
                nItem = i,
                prod = GetProduto(NotaFiscalItem),
                imposto = new imposto
                {
                    //vTotTrib = 0.17m,

                    ICMS = new ICMS
                    {
                        TipoICMS = (CRT)NotaFiscalItem.NotaFiscal.CRT == CRT.SimplesNacional ? InformarCSOSN(NotaFiscalItem) : InformarCST(NotaFiscalItem)
                    },
                    PIS = new PIS
                    {
                        TipoPIS = new PISOutr { CST = HelpExtensions.GetCode<CSTPIS>(NotaFiscalItem.Codigo_PIS), pPIS = NotaFiscalItem.pPIS, vBC = NotaFiscalItem.vProd, vPIS = NotaFiscalItem.vPIS }
                    },
                    COFINS = new COFINS
                    {
                        TipoCOFINS = new COFINSOutr { CST = HelpExtensions.GetCode<CSTCOFINS>(NotaFiscalItem.Codigo_COFINS), pCOFINS = NotaFiscalItem.pCOFINS, vBC = NotaFiscalItem.vProd, vCOFINS = NotaFiscalItem.vCOFINS }
                    },
                }
            };

            if ((ModeloDocumento)Convert.ToInt32(NotaFiscal.mod) == ModeloDocumento.NFe) //NFCe não aceita grupo "IPI"
            {
                det.imposto.IPI = new IPI()
                {
                    cEnq = 999,
                    TipoIPI = new IPITrib() { CST = HelpExtensions.GetCode<CSTIPI>(NotaFiscalItem.Codigo_IPI), pIPI = NotaFiscalItem.pIPI, vBC = NotaFiscalItem.vProd, vIPI = NotaFiscalItem.vIPI }
                };
            }

            return det;

        }

        private prod GetProduto(NotaFiscalItem NotaFiscalItem)
        {

            var p = new prod
            {
                cProd = NotaFiscalItem.cProd.PadLeft(5, '0'),
                cEAN = NotaFiscalItem.cEAN,
                xProd = NotaFiscalItem.xProd,
                NCM = NotaFiscalItem.Codigo_NCM.Replace(".", ""),
                CEST = NotaFiscalItem.Codigo_CEST,
                CFOP = NotaFiscalItem.Codigo_CFOP.Replace(".", "").ToInt(),
                uCom = NotaFiscalItem.uCom,
                qCom = (decimal)NotaFiscalItem.qCom,
                vUnCom = (decimal)NotaFiscalItem.vUnCom,
                vProd = (decimal)NotaFiscalItem.qCom * (decimal)NotaFiscalItem.vUnCom, //(decimal)NotaFiscalItem.vProd,
                vDesc = NotaFiscalItem.vDesc,
                cEANTrib = NotaFiscalItem.cEAN,
                uTrib = NotaFiscalItem.uCom,
                qTrib = (decimal)NotaFiscalItem.qCom,
                vUnTrib = (decimal)NotaFiscalItem.vUnCom,
                indTot = IndicadorTotal.ValorDoItemCompoeTotalNF,
            };

            return p;

        }

        private ICMSBasico InformarCST(NotaFiscalItem NotaFiscalItem)
        {

            switch (NotaFiscalItem.Codigo_CST)
            {
                case "00":
                    return new ICMS00
                    {
                        orig = (OrigemMercadoria)NotaFiscalItem.orig,
                        CST = Csticms.Cst00,
                        modBC = DeterminacaoBaseIcms.DbiValorOperacao,
                        pICMS = (decimal)NotaFiscalItem.pICMS,
                        vBC = (decimal)NotaFiscalItem.vProd,
                        vICMS = (decimal)NotaFiscalItem.vICMS
                    };
                case "20":
                    return new ICMS20
                    {
                        orig = (OrigemMercadoria)NotaFiscalItem.orig,
                        CST = Csticms.Cst20,
                        modBC = DeterminacaoBaseIcms.DbiValorOperacao,
                        motDesICMS = MotivoDesoneracaoIcms.MdiTaxi,
                        pICMS = (decimal)NotaFiscalItem.pICMS,
                        vBC = (decimal)NotaFiscalItem.vProd,
                        vICMS = (decimal)NotaFiscalItem.vICMS
                    };
                    //Outros casos aqui
            }

            return new ICMS10();
        }

        private ICMSBasico InformarCSOSN(NotaFiscalItem NotaFiscalItem)
        {
            switch (NotaFiscalItem.Codigo_CSOSN)
            {
                case "101":
                    return new ICMSSN101
                    {
                        orig = (OrigemMercadoria)NotaFiscalItem.orig,
                        CSOSN = Csosnicms.Csosn101,
                    };
                case "102":
                    return new ICMSSN102
                    {
                        orig = (OrigemMercadoria)NotaFiscalItem.orig,
                        CSOSN = Csosnicms.Csosn102,
                    };
                //Outros casos aqui
                default:
                    return new ICMSSN201();
            }
        }

        private total GetTotal(List<det> produtos)
        {

            var icmsTot = new ICMSTot
            {
                vProd = produtos.Sum(p => p.prod.vProd),
                vDesc = produtos.Sum(p => p.prod.vDesc ?? 0),
                vTotTrib = produtos.Sum(p => p.imposto.vTotTrib ?? 0),
                vICMSDeson = 0,
                vFCPUFDest = 0,
                vICMSUFDest = 0,
                vICMSUFRemet = 0,
                vFCP = 0,
                vFCPST = 0,
                vFCPSTRet = 0,
                vIPIDevol = 0,
            };

            foreach (var produto in produtos)
            {
                if (produto.imposto.IPI != null && produto.imposto.IPI.TipoIPI.GetType() == typeof(IPITrib))
                {
                    icmsTot.vIPI += ((IPITrib)produto.imposto.IPI.TipoIPI).vIPI ?? 0;
                }

                if (produto.imposto.PIS != null && produto.imposto.PIS.TipoPIS.GetType() == typeof(PISOutr))
                {
                    icmsTot.vPIS += ((PISOutr)produto.imposto.PIS.TipoPIS).vPIS ?? 0;
                }

                if (produto.imposto.COFINS != null && produto.imposto.COFINS.TipoCOFINS.GetType() == typeof(COFINSOutr))
                {
                    icmsTot.vCOFINS += ((COFINSOutr)produto.imposto.COFINS.TipoCOFINS).vCOFINS ?? 0;
                }

                if (produto.imposto.ICMS.TipoICMS.GetType() == typeof(ICMS00))
                {
                    icmsTot.vBC += ((ICMS00)produto.imposto.ICMS.TipoICMS).vBC;
                    icmsTot.vICMS += ((ICMS00)produto.imposto.ICMS.TipoICMS).vICMS;
                }
                if (produto.imposto.ICMS.TipoICMS.GetType() == typeof(ICMS20))
                {
                    icmsTot.vBC += ((ICMS20)produto.imposto.ICMS.TipoICMS).vBC;
                    icmsTot.vICMS += ((ICMS20)produto.imposto.ICMS.TipoICMS).vICMS;
                }
                //Outros Ifs aqui, caso vá usar as classes ICMS00, ICMS10 para totalizar
            }

            //** Regra de validação W16-10 que rege sobre o Total da NF **//
            icmsTot.vNF =
                icmsTot.vProd
                - icmsTot.vDesc
                - icmsTot.vICMSDeson.GetValueOrDefault()
                + icmsTot.vST
                + icmsTot.vFCPST.GetValueOrDefault()
                + icmsTot.vFrete
                + icmsTot.vSeg
                + icmsTot.vOutro
                + icmsTot.vII
                + icmsTot.vIPI
                + icmsTot.vIPIDevol.GetValueOrDefault();

            total t = new total { ICMSTot = icmsTot };

            NotaFiscal.vBC = icmsTot.vBC;
            NotaFiscal.vICMS = icmsTot.vICMS;
            NotaFiscal.vBCST = icmsTot.vBCST;
            NotaFiscal.vST = icmsTot.vST;
            NotaFiscal.vProd = icmsTot.vProd;
            NotaFiscal.vFrete = icmsTot.vFrete;
            NotaFiscal.vSeg = icmsTot.vSeg;
            NotaFiscal.vDesc = icmsTot.vDesc;
            NotaFiscal.vII = icmsTot.vII;
            NotaFiscal.vIPI = icmsTot.vIPI;
            NotaFiscal.vPIS = icmsTot.vPIS;
            NotaFiscal.vCOFINS = icmsTot.vCOFINS;
            NotaFiscal.vOutro = icmsTot.vOutro;
            NotaFiscal.vNF = icmsTot.vNF;


            return t;
        }

        private transp GetTransporte()
        {
            //var volumes = new List<vol> {GetVolume(), GetVolume()};

            var t = new transp
            {
                modFrete = ModalidadeFrete.mfSemFrete //NFCe: Não pode ter frete
                //vol = volumes 
            };

            return t;

        }

        private vol GetVolume()
        {
            var v = new vol
            {
                esp = "teste de espécie",
                lacres = new List<lacres> { new lacres { nLacre = "123456" } }
            };

            return v;
        }

        private cobr GetCobranca(ICMSTot icmsTot)
        {
            decimal valorParcela = ((icmsTot.vProd - icmsTot.vDesc) / 2);

            var c = new cobr
            {
                fat = new fat { nFat = "12345678910", vLiq = icmsTot.vNF, vOrig = icmsTot.vNF, vDesc = 0m },
                dup = new List<dup>
                {
                    new dup {nDup = "001", dVenc = DateTime.Now.AddDays(30), vDup = valorParcela},
                    new dup {nDup = "002", dVenc = DateTime.Now.AddDays(60), vDup = icmsTot.vNF - valorParcela}
                }
            };

            return c;
        }

        private List<pag> GetPagamento(ICMSTot icmsTot)
        {

            decimal valorPagto = icmsTot.vNF; //(icmsTot.vNF / 2);

            var pag = new List<pag>
            {
                new pag
                {
                    detPag = new List<detPag>
                    {
                        new detPag {tPag = Classes.Informacoes.Pagamento.FormaPagamento.fpOutro, vPag = valorPagto},
                        //new detPag {tPag = Classes.Informacoes.Pagamento.FormaPagamento.fpCreditoLoja, vPag = icmsTot.vNF - valorPagto}
                    }
                }
            };

            return pag;

        }


    }

    public class HelpNFe
    {

        public static void NfeStatusServico(ConfiguracaoServico CfgServico, X509Certificate2 X509Certificate2)
        {

            var Retorno = Servicos.NfeStatusServico(CfgServico, X509Certificate2);

            if (Retorno.Retorno.cStat != 107)
            {
                throw new Exception(Retorno.Retorno.cStat + " - " + Retorno.Retorno.xMotivo);
            }
        }

        public static RetornoNfeDistDFeInt ConsultaDistribuicaoNFe(ConfiguracaoServico CfgServico, long? ultNSU = null, string chNFe = null)
        {

            RetornoNfeDistDFeInt DistDFe;

            int tentativas = 0;

        tentaNovamente:

            if (chNFe == null)
                DistDFe = Servicos.NfeDistDFeInteresse(CfgServico, ultNSU ?? 0);
            else
                DistDFe = Servicos.NfeDistDFeInteresse(CfgServico, chNFe: chNFe);


            //Nenhum documento localizado
            if (DistDFe.Retorno.cStat == 137)
                return null;


            if (DistDFe.Retorno.loteDistDFeInt == null)
            {
                tentativas++;

                if (tentativas == 20)
                {
                    throw new Exception(DistDFe.Retorno.xMotivo);
                }

                goto tentaNovamente;
            }

            return DistDFe;

        }

        public static List<nfeProc> NFeAutorizacao(ConfiguracaoServico CfgServico, X509Certificate2 X509Certificate2, List<Classes.NFe> NFes)
        {

            //int tentativas = 0;

            //tentaNovamente:


            var RetornoNFeAutorizacao = Servicos.NfeAutorizacao(CfgServico, X509Certificate2, NFes);


            RetornoNFeRetAutorizacao NFeRetAutorizacao;

            while (true)
            {

                Thread.Sleep(1200);

                NFeRetAutorizacao = Servicos.NFeRetAutorizacao(CfgServico, X509Certificate2, RetornoNFeAutorizacao.Retorno.infRec.nRec);

                if (NFeRetAutorizacao.Retorno.cStat == 104)
                {
                    break;
                }

            }


            //var NFeRetAutorizacao = Servicos.NFeRetAutorizacao(CfgServico, RetornoNFeAutorizacao.Retorno.infRec.nRec);


            //if (DistDFe.Retorno.loteDistDFeInt == null)
            //{
            //    tentativas++;

            //    if (tentativas == 20)
            //    {
            //        throw new Exception(DistDFe.Retorno.xMotivo);
            //    }

            //    goto tentaNovamente;
            //}

            var NfeProcs = new List<nfeProc>();


            var db = new Context();

            var Empresa = db.Empresa.FirstOrDefault();

            foreach (var item in NFeRetAutorizacao.Retorno.protNFe)
            {

                var NFe = NFes.FirstOrDefault(c => c.infNFe.Id == "NFe" + item.infProt.chNFe);

                var NotaFiscal = db.NotaFiscal.FirstOrDefault(c => c.nNF == NFe.infNFe.ide.nNF && c.serie == NFe.infNFe.ide.serie);

                NotaFiscal.cStat = item.infProt.cStat;
                NotaFiscal.xMotivo = item.infProt.xMotivo;
                NotaFiscal.nProt = item.infProt.nProt;

                NotaFiscal.chNFe = item.infProt.chNFe;

                if (item.infProt.cStat == 100)
                {

                    if (NotaFiscal.nNF > Empresa.NFe_Numero)
                    {
                        Empresa.NFe_Numero = NotaFiscal.nNF;
                        db.Empresa.Update(Empresa);
                    }


                }

                NfeProcs.Add(new nfeProc()
                {
                    NFe = NFe,
                    protNFe = item,
                    versao = item.versao
                });

                db.NotaFiscal.Update(NotaFiscal);

                

            }

            db.SaveChanges();

            return NfeProcs;

        }

        public static void CienciaDaOperacao(ConfiguracaoServico CfgServico, string[] chNFe)
        {

            var Retorno = Servicos.RecepcaoEventoManifestacaoDestinatario(CfgServico, 1, chNFe, NFeTipoEvento.TeMdCienciaDaOperacao);

            foreach (var item in Retorno.Retorno.retEvento)
            {
                switch (item.infEvento.cStat)
                {
                    case 135: //"Evento registrado e vinculado a NF-e"
                        continue;
                    case 573: //"Rejeicao: Duplicidade de evento"
                        continue;
                    case 575: //"Rejeicao: O autor do evento diverge do destinatario da NF-e"
                        continue;
                    case 650: //"Rejeicao: Evento de Ciencia da Operacao para NFe cancelada ou denegada"
                        continue;

                    default:
                        //Nenhuma tratativa
                        throw new Exception(item.infEvento.xMotivo + "\n\nChave de acesso: " + item.infEvento.chNFe);
                }
            }

        }

        public static void ConfirmacaoDaOperacao(ConfiguracaoServico CfgServico, string[] chNFe)
        {

            var Retorno = Servicos.RecepcaoEventoManifestacaoDestinatario(CfgServico, 1, chNFe, NFeTipoEvento.TeMdConfirmacaoDaOperacao);

            foreach (var item in Retorno.Retorno.retEvento)
            {
                switch (item.infEvento.cStat)
                {
                    case 135: //"Evento registrado e vinculado a NF-e"
                        continue;
                    case 573: //"Rejeicao: Duplicidade de evento"
                        continue;

                    default:
                        //Nenhuma tratativa
                        throw new Exception(item.infEvento.xMotivo + "\n\nChave de acesso: " + item.infEvento.chNFe);
                }
            }

        }

        public static void DesconhecimentoDaOperacao(ConfiguracaoServico CfgServico, string[] chNFe)
        {

            var Retorno = Servicos.RecepcaoEventoManifestacaoDestinatario(CfgServico, 1, chNFe, NFeTipoEvento.TeMdDesconhecimentoDaOperacao);

            foreach (var item in Retorno.Retorno.retEvento)
            {
                switch (item.infEvento.cStat)
                {
                    case 135: //"Evento registrado e vinculado a NF-e"
                        continue;
                    case 573: //"Rejeicao: Duplicidade de evento"
                        continue;
                    case 999: //"Rejeicao: Desconhecido"
                        continue;

                    default:
                        //Nenhuma tratativa
                        throw new Exception(item.infEvento.xMotivo + "\n\nChave de acesso: " + item.infEvento.chNFe);
                }
            }

        }


    }

    public class Servicos
    {

        #region Serviços
        public static RetornoNfeStatusServico NfeStatusServico(ConfiguracaoServico CfgServico, X509Certificate2 X509Certificate2)
        {
            int tentativas = 0;

        tentaNovamente:

            try
            {

                tentativas++;

                CfgServico.VersaoNfeStatusServico = VersaoServico.Versao400;
                CfgServico.tpEmis = TipoEmissao.teNormal;
                CfgServico.ModeloDocumento = ModeloDocumento.NFe;

                return new ServicosNFe(CfgServico, X509Certificate2).NfeStatusServico();

            }
            catch (Exception ex)
            {
                //Tenta 20x se ocorrer erro
                if (tentativas == 20) throw new Exception(ex.Message);
                goto tentaNovamente;
            }
        }

        public static RetornoNfeConsultaCadastro NfeConsultaCadastro(ConfiguracaoServico CfgServico, X509Certificate2 X509Certificate2, string UF, string CNPJCPF)
        {
            int tentativas = 0;

        tentaNovamente:

            try
            {

                tentativas++;

                var CfgConsulta = new ConfiguracaoServico
                {
                    DiretorioSchemas = CfgServico.DiretorioSchemas,
                    Certificado = CfgServico.Certificado,
                    VersaoNfeConsultaCadastro = VersaoServico.Versao400,
                    tpEmis = TipoEmissao.teNormal,
                    tpAmb = TipoAmbiente.Producao,
                    ModeloDocumento = ModeloDocumento.NFe,
                    cUF = new Entidades.Estado().SiglaParaEstado(UF)
                };

                return new ServicosNFe(CfgConsulta).NfeConsultaCadastro(UF, ConsultaCadastroTipoDocumento.Cnpj, CNPJCPF);

            }
            catch (Exception ex)
            {
                //Tenta 20x se ocorrer erro
                if (tentativas == 20) throw new Exception(ex.Message);
                goto tentaNovamente;
            }
        }

        public static RetornoNFeAutorizacao NfeAutorizacao(ConfiguracaoServico CfgServico, X509Certificate2 X509Certificate2, List<NFe.Classes.NFe> NFes)
        {
            int tentativas = 0;

        tentaNovamente:

            try
            {

                tentativas++;

                var CfgAutorizacao = new ConfiguracaoServico
                {
                    DiretorioSchemas = CfgServico.DiretorioSchemas,
                    VersaoNFeAutorizacao = VersaoServico.Versao400,
                    tpEmis = TipoEmissao.teNormal,
                    tpAmb = TipoAmbiente.Homologacao,
                    ModeloDocumento = ModeloDocumento.NFe,
                    cUF = new Entidades.Estado().SiglaParaEstado("GO"),
                };

                return new ServicosNFe(CfgAutorizacao, X509Certificate2).NFeAutorizacao(1, IndicadorSincronizacao.Assincrono, NFes);

            }
            catch (Exception ex)
            {
                //Tenta 20x se ocorrer erro
                if (tentativas == 20) throw new Exception(ex.Message);
                goto tentaNovamente;
            }
        }

        public static RetornoNFeRetAutorizacao NFeRetAutorizacao(ConfiguracaoServico CfgServico, X509Certificate2 X509Certificate2, string recibo)
        {
            int tentativas = 0;

        tentaNovamente:

            try
            {

                tentativas++;

                var CfgAutorizacao = new ConfiguracaoServico
                {
                    DiretorioSchemas = CfgServico.DiretorioSchemas,
                    VersaoNFeRetAutorizacao = VersaoServico.Versao400,
                    tpEmis = TipoEmissao.teNormal,
                    tpAmb = TipoAmbiente.Homologacao,
                    ModeloDocumento = ModeloDocumento.NFe,
                    cUF = new Entidades.Estado().SiglaParaEstado("GO"),
                };

                return new ServicosNFe(CfgAutorizacao, X509Certificate2).NFeRetAutorizacao(recibo);

            }
            catch (Exception ex)
            {
                //Tenta 20x se ocorrer 
                if (tentativas == 20) throw new Exception(ex.Message);
                goto tentaNovamente;
            }
        }


        public static RetornoRecepcaoEvento RecepcaoEventoManifestacaoDestinatario(ConfiguracaoServico CfgServico, int SequenciaEvento, string[] chNFe, NFeTipoEvento NFeTipoEvento, string Justificativa = null)
        {

            int tentativas = 0;

        tentaNovamente:

            try
            {

                tentativas++;

                CfgServico.VersaoRecepcaoEventoManifestacaoDestinatario = VersaoServico.Versao400;
                CfgServico.tpEmis = TipoEmissao.teNormal;
                CfgServico.ModeloDocumento = ModeloDocumento.NFe;

                return new ServicosNFe(CfgServico).RecepcaoEventoManifestacaoDestinatario(1, SequenciaEvento, chNFe, NFeTipoEvento, "04058687000177", Justificativa);

            }
            catch (Exception ex)
            {
                //Tenta 20x se ocorrer erro
                if (tentativas == 20) throw new Exception(ex.Message);
                goto tentaNovamente;
            }
        }
        public static RetornoNfeDistDFeInt NfeDistDFeInteresse(ConfiguracaoServico CfgServico, long ultNSU = 0, long nSU = 0, string chNFe = "")
        {

            int tentativas = 0;

        tentaNovamente:

            try
            {

                tentativas++;

                CfgServico.VersaoNFeDistribuicaoDFe = VersaoServico.Versao100;
                CfgServico.tpEmis = TipoEmissao.teNormal;
                CfgServico.ModeloDocumento = ModeloDocumento.NFe;

                return new ServicosNFe(CfgServico).NfeDistDFeInteresse("52", "04058687000177", ultNSU.ToString(), nSU.ToString(), chNFe);

            }
            catch (Exception ex)
            {
                //Tenta 20x se ocorrer erro
                if (tentativas == 20) throw new Exception(ex.Message);
                goto tentaNovamente;
            }
        }
        #endregion

    }

}