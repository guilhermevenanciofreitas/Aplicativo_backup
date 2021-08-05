using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.Utils.WebServices;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Fiscal.NotasFiscais
{
    public partial class ViewNotaFiscalPage : ComponentBase
    {

        public int? NotaFiscalID { get; set; }

        public NotaFiscal ViewModel = new NotaFiscal();

        [Parameter] public ListItemViewLayout<NotaFiscal> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public DropDownList DplStatus { get; set; }

        public FileDialog FileDialog { get; set; }
        public TextBox TxtArquivo { get; set; }

        public TextBox TxtChaveAcesso { get; set; }
        public DropDownList DplModelo { get; set; }
        public TextBox TxtNaturezaOperacao { get; set; }

        public TextBox TxtNumero { get; set; }
        public TextBox TxtSerie { get; set; }
        public DropDownList DplTipo { get; set; }
        public DropDownList DplFinalidade { get; set; }
        public DateTimePicker DtpEmissao { get; set; }
        public DateTimePicker DtpEntradaSaida { get; set; }

        public DropDownList DplAmbiente { get; set; }
        public DropDownList DplTipoEmissao { get; set; }
        public DropDownList DplIndicadorPresenca { get; set; }
        public DropDownList DplCRT { get; set; }


        public NumericBox TxtBaseCalculoICMS { get; set; }
        public NumericBox TxtValorICMS { get; set; }
        public NumericBox TxtBaseCalculoICMSST { get; set; }
        public NumericBox TxtValorICMSST { get; set; }
        public NumericBox TxtAproxTributos { get; set; }
        public NumericBox TxtTotalProdutos { get; set; }

        public NumericBox TxtValorFrete { get; set; }
        public NumericBox TxtValorSeguro { get; set; }
        public NumericBox TxtDesconto { get; set; }
        public NumericBox TxtOutrasDespAcess { get; set; }
        public NumericBox TxtValorIPI { get; set; }
        public NumericBox TxtTotalNota { get; set; }

        public TextArea TxtInformacoesComplementares { get; set; }


        public TextBox Emit_TxtCNPJ { get; set; }
        public TextBox Emit_TxtRazaoSocial { get; set; }
        public TextBox Emit_TxtInscricaoEstadual { get; set; }
        public TextBox Emit_TxtTelefone { get; set; }
        public TextBoxCEP Emit_TxtCEP { get; set; }
        public TextBox Emit_TxtLogradouro { get; set; }
        public TextBox Emit_TxtNumero { get; set; }
        public TextBox Emit_TxtComplemento { get; set; }
        public TextBox Emit_TxtBairro { get; set; }
        public DropDownList Emit_TxtUF { get; set; }
        public ViewPesquisa<Municipio> Emit_ViewPesquisaMunicipio { get; set; }



        public TextBox Dest_TxtCNPJ { get; set; }
        public TextBox Dest_TxtRazaoSocial { get; set; }
        public TextBox Dest_TxtInscricaoEstadual { get; set; }
        public TextBoxCEP Dest_TxtCEP { get; set; }
        public TextBox Dest_TxtLogradouro { get; set; }
        public TextBox Dest_TxtNumero { get; set; }
        public TextBox Dest_TxtComplemento { get; set; }
        public TextBox Dest_TxtBairro { get; set; }
        public DropDownList Dest_TxtUF { get; set; }
        public ViewPesquisa<Municipio> Dest_ViewPesquisaMunicipio { get; set; }


        public DropDownList Transp_TxtUF { get; set; }
        public ViewPesquisa<Municipio> Transp_ViewPesquisaMunicipio { get; set; }


        public ViewNotaFiscalItem ViewNotaFiscalItem { get; set; }

        #endregion

        protected void InitializeComponents()
        {

            DplModelo.LoadDropDownList("Codigo", "Descricao", new DropDownListItem(null, "[Selecione]"), HelpParametros.Parametros.NotaFiscalModelo.OrderBy(c => c.Codigo).ToList());


            DplStatus.Items.Clear();
            DplStatus.Add(null, "[Selecione]");
            DplStatus.Add("100", "100 - Autorizado o uso da NF-e");
            DplStatus.Add("102", "102 - Inutilização de número homologado");
            DplStatus.Add("103", "103 - Lote recebido com sucesso");
            DplStatus.Add("104", "104 - Lote processado");
            DplStatus.Add("105", "105 - Lote em processamento");
            DplStatus.Add("106", "106 - Lote não localizado");
            DplStatus.Add("108", "108 - Serviço Paralisado Momentaneamente");
            DplStatus.Add("109", "109 - Serviço Paralisado sem Previsão");
            DplStatus.Add("110", "110 - Uso Denegado");
            DplStatus.Add("111", "111 - Consulta cadastro com uma ocorrência");
            DplStatus.Add("112", "112 - Consulta cadastro com mais de uma ocorrência");
            DplStatus.Add("101", "101 - Cancelamento de NF-e homologado");

            DplTipo.Items.Clear();
            DplTipo.Add(null, "[Selecione]");
            DplTipo.Add("0", "0 - Entrada");
            DplTipo.Add("1", "1 - Saída");

            DplFinalidade.Items.Clear();
            DplFinalidade.Add(null, "[Selecione]");
            DplFinalidade.Add("1", "1 - Normal");
            DplFinalidade.Add("2", "2 - Complementar");
            DplFinalidade.Add("3", "3 - Ajuste");
            DplFinalidade.Add("4", "4 - Devolução de merc.");

            DplAmbiente.Items.Clear();
            DplAmbiente.Add(null, "[Selecione]");
            DplAmbiente.Add("1", "1 - Produção");
            DplAmbiente.Add("2", "2 - Homologação");

            DplTipoEmissao.Items.Clear();
            DplTipoEmissao.Add(null, "[Selecione]");
            DplTipoEmissao.Add("1", "1 - Emissão normal");
            DplTipoEmissao.Add("2", "2 - Contingência FS-IA");
            DplTipoEmissao.Add("3", "3 - Contingência SCAN");
            DplTipoEmissao.Add("4", "4 - Contingência DPEC");
            DplTipoEmissao.Add("5", "5 - Contingência FS-DA");
            DplTipoEmissao.Add("6", "6 - Contingência SVC-AN");
            DplTipoEmissao.Add("7", "7 - Contingência SVC-RS");
            DplTipoEmissao.Add("9", "9 - Contingência off-line da NFC-e");

            DplIndicadorPresenca.Items.Clear();
            DplIndicadorPresenca.Add(null, "[Selecione]");
            DplIndicadorPresenca.Add("0", "0 - Não se aplica");
            DplIndicadorPresenca.Add("1", "1 - Operação presencial");
            DplIndicadorPresenca.Add("2", "2 - Operação não presencial, pela Internet");
            DplIndicadorPresenca.Add("3", "3 - Operação não presencial, Teleatendimento");
            DplIndicadorPresenca.Add("4", "4 - NFC-e em operação com entrega a domicílio");
            DplIndicadorPresenca.Add("5", "5 - Operação presencial, fora do estabelecimento");
            DplIndicadorPresenca.Add("9", "9 - Operação não presencial, outros");

            DplCRT.Items.Clear();
            DplCRT.Add(null, "[Selecione]");
            DplCRT.Add("1", "1 - Simples Nacional");
            DplCRT.Add("2", "2 - Simples Nacional – excesso de sublimite de receita bruta");
            DplCRT.Add("3", "3 - Regime Normal");

            Emit_TxtUF.LoadDropDownList("EstadoID", "UF", null, HelpParametros.Parametros.Estado.OrderBy(c => c.UF).ToList());
            Dest_TxtUF.LoadDropDownList("EstadoID", "UF", null, HelpParametros.Parametros.Estado.OrderBy(c => c.UF).ToList());

        }

        protected async Task Page_Load(object args)
        {

            InitializeComponents();

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<NotaFiscal>();

            Query.AddInclude("NotaFiscalItem");

            Query.AddInclude("NotaFiscalItem.CFOP");
            Query.AddInclude("NotaFiscalItem.NCM");
            Query.AddInclude("NotaFiscalItem.CEST");

            Query.AddInclude("NotaFiscalItem.CST_ICMS");
            Query.AddInclude("NotaFiscalItem.CSOSN_ICMS");
            Query.AddInclude("NotaFiscalItem.CST_IPI");
            Query.AddInclude("NotaFiscalItem.CST_PIS");
            Query.AddInclude("NotaFiscalItem.CST_COFINS");

            Query.AddWhere("NotaFiscalID == @0", ((NotaFiscal)args).NotaFiscalID);

            ViewModel = await Query.FirstOrDefault();


            #region Principal
            NotaFiscalID = ViewModel.NotaFiscalID;


            DplStatus.SelectedValue = ViewModel.cStat.ToStringOrNull();
            TxtChaveAcesso.Text = ViewModel.chNFe.StringFormat("#### #### #### #### #### #### #### #### #### #### ####");
            DplModelo.SelectedValue = ViewModel.mod.ToStringOrNull();
            TxtNaturezaOperacao.Text = ViewModel.natOp.ToStringOrNull();

            TxtNumero.Text = ViewModel.nNF.ToStringOrNull();
            TxtSerie.Text = ViewModel.serie.ToStringOrNull();
            DplTipo.SelectedValue = ViewModel.tpNF.ToStringOrNull();
            DplFinalidade.SelectedValue = ViewModel.finNFe.ToStringOrNull();
            DtpEmissao.Value = ViewModel.dhEmi;
            DtpEntradaSaida.Value = ViewModel.dhSaiEnt;

            DplAmbiente.SelectedValue = ViewModel.tpAmb.ToStringOrNull();
            DplTipoEmissao.SelectedValue = ViewModel.tpEmis.ToStringOrNull();
            DplIndicadorPresenca.SelectedValue = ViewModel.indPres.ToStringOrNull();
            DplCRT.SelectedValue = ViewModel.CRT.ToStringOrNull();

            TxtBaseCalculoICMS.Value = ViewModel.vBC ?? 0;
            TxtValorICMS.Value = ViewModel.vICMS ?? 0;
            TxtBaseCalculoICMSST.Value = ViewModel.vBCST ?? 0;
            TxtValorICMSST.Value = ViewModel.vST ?? 0;
            TxtAproxTributos.Value = ViewModel.vTotTrib ?? 0;
            TxtTotalProdutos.Value = ViewModel.vProd ?? 0;

            TxtValorFrete.Value = ViewModel.vFrete ?? 0;
            TxtValorSeguro.Value = ViewModel.vSeg ?? 0;
            TxtDesconto.Value = ViewModel.vDesc ?? 0;
            TxtOutrasDespAcess.Value = ViewModel.vOutro ?? 0;
            TxtValorIPI.Value = ViewModel.vIPI ?? 0;
            TxtTotalNota.Value = ViewModel.vNF ?? 0;



            TxtInformacoesComplementares.Text = ViewModel.infCpl.ToStringOrNull();
            #endregion


            Emit_TxtCNPJ.Text = ViewModel.CNPJCPF.ToStringOrNull();
            Emit_TxtRazaoSocial.Text = ViewModel.xNome.ToStringOrNull();
            Emit_TxtInscricaoEstadual.Text = ViewModel.IE.ToStringOrNull();
            Emit_TxtTelefone.Text = ViewModel.Fone.ToStringOrNull();
            Emit_TxtCEP.Text = ViewModel.CEP.ToStringOrNull();
            Emit_TxtLogradouro.Text = ViewModel.xLgr.ToStringOrNull();
            Emit_TxtNumero.Text = ViewModel.nro.ToStringOrNull();
            Emit_TxtComplemento.Text = ViewModel.xCpl.ToStringOrNull();
            Emit_TxtBairro.Text = ViewModel.xBairro.ToStringOrNull();
            Emit_TxtUF.SelectedValue = HelpParametros.Parametros.Estado.FirstOrDefault(c => c.UF == ViewModel.UF.ToStringOrNull())?.EstadoID.ToStringOrNull();
            Emit_ViewPesquisaMunicipio.Value = ViewModel.cMun.ToStringOrNull();
            Emit_ViewPesquisaMunicipio.Text = ViewModel.xMun;


            Dest_TxtCNPJ.Text = ViewModel.dest_CNPJCPF.ToStringOrNull();
            Dest_TxtRazaoSocial.Text = ViewModel.dest_xNome.ToStringOrNull();
            Dest_TxtInscricaoEstadual.Text = ViewModel.dest_IE.ToStringOrNull();
            Dest_TxtCEP.Text = ViewModel.dest_CEP.ToStringOrNull();
            Dest_TxtLogradouro.Text = ViewModel.dest_xLgr.ToStringOrNull();
            Dest_TxtNumero.Text = ViewModel.dest_nro.ToStringOrNull();
            Dest_TxtComplemento.Text = ViewModel.dest_xCpl.ToStringOrNull();
            Dest_TxtBairro.Text = ViewModel.dest_xBairro.ToStringOrNull();
            Dest_TxtUF.SelectedValue = HelpParametros.Parametros.Estado.FirstOrDefault(c => c.UF == ViewModel.dest_UF.ToStringOrNull())?.EstadoID.ToStringOrNull();
            Dest_ViewPesquisaMunicipio.Value = ViewModel.dest_cMun.ToStringOrNull();
            Dest_ViewPesquisaMunicipio.Text = ViewModel.dest_xMun;



            ViewNotaFiscalItem.ListView.Items = ViewModel.NotaFiscalItem.ToList();

        }

        protected async Task BtnLimpar_Click()
        {

            ViewModel = new NotaFiscal();

            EditItemViewLayout.LimparCampos(this);


            NotaFiscalID = null;

            DplTipo.SelectedValue = "0";
            DplFinalidade.SelectedValue = "1";

            DplAmbiente.SelectedValue = "1";
            DplTipoEmissao.SelectedValue = "1";


            Emit_ViewPesquisaMunicipio.Clear();
            Dest_ViewPesquisaMunicipio.Clear();
            Transp_ViewPesquisaMunicipio.Clear();


            ViewNotaFiscalItem.ListView.Items = new List<NotaFiscalItem>();

            await TabSet.Active("Principal");

            TxtChaveAcesso.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtLogin.Text))
            //{
            //    await TabSet.Active("Principal");
            //    throw new EmptyException("Informe o login!", TxtLogin.Element);
            //}

            //if (TxtSenha.Text != TxtConfirmarSenha.Text)
            //{
            //    await TabSet.Active("Principal");
            //    throw new EmptyException("A confirmação da senha está diferente da senha informada!", TxtConfirmarSenha.Element);
            //}


            #region Principal
            ViewModel.NotaFiscalID = NotaFiscalID;

            ViewModel.cStat = DplStatus.SelectedValue.ToIntOrNull();
            ViewModel.chNFe = TxtChaveAcesso.Text?.Replace(" ", "");
            ViewModel.mod = DplModelo.SelectedValue.ToStringOrNull();
            ViewModel.natOp = TxtNaturezaOperacao.Text.ToStringOrNull();

            ViewModel.nNF = TxtNumero.Text.ToIntOrNull();
            ViewModel.serie = TxtSerie.Text.ToIntOrNull();

            ViewModel.tpNF = DplTipo.SelectedValue.ToByteOrNull();
            ViewModel.finNFe = DplFinalidade.SelectedValue.ToByteOrNull();
            ViewModel.dhEmi = DtpEmissao.Value;
            ViewModel.dhSaiEnt = DtpEntradaSaida.Value;

            ViewModel.tpAmb = DplAmbiente.SelectedValue.ToByteOrNull();
            ViewModel.tpEmis = DplTipoEmissao.SelectedValue.ToIntOrNull();
            ViewModel.indPres = DplIndicadorPresenca.SelectedValue.ToByteOrNull();
            ViewModel.CRT = DplCRT.SelectedValue.ToIntOrNull();


            ViewModel.infCpl = TxtInformacoesComplementares.Text.ToStringOrNull();


            ViewModel.vBC = TxtBaseCalculoICMS.Value;
            ViewModel.vICMS = TxtValorICMS.Value;
            ViewModel.vBCST = TxtBaseCalculoICMSST.Value;
            ViewModel.vST = TxtValorICMSST.Value;
            ViewModel.vTotTrib = TxtAproxTributos.Value;
            ViewModel.vProd = TxtTotalProdutos.Value;

            ViewModel.vFrete = TxtValorFrete.Value;
            ViewModel.vSeg = TxtValorSeguro.Value;
            ViewModel.vDesc = TxtDesconto.Value;
            ViewModel.vOutro = TxtOutrasDespAcess.Value;
            ViewModel.vIPI = TxtValorIPI.Value;
            ViewModel.vNF = TxtTotalNota.Value;

            #endregion

            ViewModel.CNPJCPF = Emit_TxtCNPJ.Text.ToStringOrNull();
            ViewModel.xNome = Emit_TxtRazaoSocial.Text.ToStringOrNull();
            ViewModel.IE = Emit_TxtInscricaoEstadual.Text.ToStringOrNull();
            ViewModel.Fone = Emit_TxtTelefone.Text.ToStringOrNull();
            ViewModel.CEP = Emit_TxtCEP.Text.ToStringOrNull();
            ViewModel.xLgr = Emit_TxtLogradouro.Text.ToStringOrNull();
            ViewModel.nro = Emit_TxtNumero.Text.ToStringOrNull();
            ViewModel.xCpl = Emit_TxtComplemento.Text.ToStringOrNull();
            ViewModel.xBairro = Emit_TxtBairro.Text.ToStringOrNull();
            ViewModel.UF = Emit_TxtUF.SelectedValue != null ? Emit_TxtUF.SelectedText.ToStringOrNull() : null;
            ViewModel.xMun = Emit_ViewPesquisaMunicipio.Text;


            ViewModel.dest_CNPJCPF = Dest_TxtCNPJ.Text.ToStringOrNull();
            ViewModel.dest_xNome = Dest_TxtRazaoSocial.Text.ToStringOrNull();
            ViewModel.dest_IE = Dest_TxtInscricaoEstadual.Text.ToStringOrNull();
            ViewModel.dest_CEP = Dest_TxtCEP.Text.ToStringOrNull();
            ViewModel.dest_xLgr = Dest_TxtLogradouro.Text.ToStringOrNull();
            ViewModel.dest_nro = Dest_TxtNumero.Text.ToStringOrNull();
            ViewModel.dest_xCpl = Dest_TxtComplemento.Text.ToStringOrNull();
            ViewModel.dest_xBairro = Dest_TxtBairro.Text.ToStringOrNull();
            ViewModel.dest_UF = Dest_TxtUF.SelectedValue != null ? Dest_TxtUF.SelectedText.ToStringOrNull() : null;
            ViewModel.dest_xMun = Dest_ViewPesquisaMunicipio.Text;

            ViewModel.NotaFiscalItem = ViewNotaFiscalItem.ListView.Items.ToList();

            foreach (var item in ViewModel.NotaFiscalItem)
            {
                item.CFOP = null;
                item.NCM = null;
                item.CEST = null;
                item.CST_ICMS = null;
                item.CSOSN_ICMS = null;
                item.CST_IPI = null;
                item.CST_PIS = null;
                item.CST_COFINS = null;
            }

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            var Changes = await HelpUpdate.SaveChanges();

            ViewModel = HelpUpdate.Bind<NotaFiscal>(Changes[0]);

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                await Page_Load(ViewModel);
            }
            else
            {
                await EditItemViewLayout.ViewModal.Hide();
            }

        }

        protected async Task BtnExcluir_Click()
        {

            //await Excluir(new List<int> { TxtCodigo.Text.ToInt() });

            await EditItemViewLayout.ViewModal.Hide();

        }

        public async Task Excluir(List<int> args)
        {

            var Query = new HelpQuery<NotaFiscal>();

            Query.AddWhere("NotaFiscalID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                //item.Ativo = false;
            }

            //await Query.Update(ViewModel, false);

        }

        public async Task Enviar(List<int> args)
        {

            var Query = new HelpQuery<NotaFiscal>();

            Query.AddWhere("NotaFiscalID IN (" + string.Join(",", args.ToArray()) + ")");

            var ListNotaFiscal = await Query.ToList();

            foreach(var item in ListNotaFiscal)
            {
                if (item.nProt != null)
                {
                    throw new ErrorException("Nota fiscal número " + item.nNF + " série " + item.serie + " já está autorizado o uso!");
                }
            }

            var Request = new Request();

            Request.Parameters.Add(new Parameters("EmpresaID", HelpParametros.Parametros.EmpresaLogada.EmpresaID));
            Request.Parameters.Add(new Parameters("NotaFiscal", ListNotaFiscal));

            var NotaFiscal = await HelpHttp.Send<List<NotaFiscal>>("api/NotaFiscal/Enviar", Request);


        }

        protected async Task BtnArquivo_Click()
        {

            var Arquivo = (await FileDialog.OpenFileDialog(Multiple: false, Accept: new string[] { ".xml" })).FirstOrDefault();

            if (Arquivo != null)
            {

                TxtArquivo.Text = Arquivo.Nome;

            }

        }


        #region Emitente
        protected void Emit_TxtCEP_Success(object args)
        {

            var ViaCEP = (ViaCEP)args;

            Emit_TxtLogradouro.Text = ViaCEP.logradouro.ToStringOrNull();
            Emit_TxtBairro.Text = ViaCEP.bairro.ToStringOrNull();
            Emit_TxtUF.SelectedValue = HelpParametros.Parametros.Estado.FirstOrDefault(c => c.UF == ViaCEP.UF.ToStringOrNull())?.EstadoID.ToStringOrNull();
            Emit_ViewPesquisaMunicipio.Value = ViaCEP.MunicipioID.ToStringOrNull();
            Emit_ViewPesquisaMunicipio.Text = ViaCEP.municipio.ToStringOrNull();
            ViewModel.cMun = ViaCEP.ibge.ToIntOrNull();

            Emit_TxtNumero.Focus();

        }
        protected void Emit_TxtCEP_Error(object args)
        {

            App.JSRuntime.InvokeVoidAsync("alert", args.ToString());

            Emit_TxtLogradouro.Text = null;
            Emit_TxtBairro.Text = null;
            Emit_ViewPesquisaMunicipio.Clear();

            Emit_TxtCEP.Focus();

        }
        protected void Emit_TxtUF_Change(object args)
        {

            var Predicate = "EstadoID == @0";

            Emit_ViewPesquisaMunicipio.Where.Remove(Emit_ViewPesquisaMunicipio.Where.FirstOrDefault(c => c.Predicate == Predicate));
            Emit_ViewPesquisaMunicipio.AddWhere(Predicate, ((ChangeEventArgs)args).Value.ToString());

            Emit_ViewPesquisaMunicipio.Clear();

        }
        protected void Emit_ViewPesquisaMunicipio_Change(object args)
        {
            ViewModel.cMun = ((Municipio)args)?.IBGE;
        }
        #endregion

        #region Destinatario
        protected void Dest_TxtCEP_Success(object args)
        {

            var ViaCEP = (ViaCEP)args;

            Dest_TxtLogradouro.Text = ViaCEP.logradouro.ToStringOrNull();
            Dest_TxtBairro.Text = ViaCEP.bairro.ToStringOrNull();
            Dest_TxtUF.SelectedValue = HelpParametros.Parametros.Estado.FirstOrDefault(c => c.UF == ViaCEP.UF.ToStringOrNull())?.EstadoID.ToStringOrNull();
            Dest_ViewPesquisaMunicipio.Value = ViaCEP.MunicipioID.ToStringOrNull();
            Dest_ViewPesquisaMunicipio.Text = ViaCEP.municipio.ToStringOrNull();
            ViewModel.dest_cMun = ViaCEP.ibge.ToIntOrNull();

            Dest_TxtNumero.Focus();

        }
        protected void Dest_TxtCEP_Error(object args)
        {

            App.JSRuntime.InvokeVoidAsync("alert", args.ToString());

            Dest_TxtLogradouro.Text = null;
            Dest_TxtBairro.Text = null;
            Dest_ViewPesquisaMunicipio.Clear();

            Dest_TxtCEP.Focus();

        }
        protected void Dest_TxtUF_Change(object args)
        {

            var Predicate = "EstadoID == @0";

            Dest_ViewPesquisaMunicipio.Where.Remove(Dest_ViewPesquisaMunicipio.Where.FirstOrDefault(c => c.Predicate == Predicate));
            Dest_ViewPesquisaMunicipio.AddWhere(Predicate, ((ChangeEventArgs)args).Value.ToString());

            Dest_ViewPesquisaMunicipio.Clear();

        }
        protected void Dest_ViewPesquisaMunicipio_Change(object args)
        {
            ViewModel.dest_cMun = ((Municipio)args)?.IBGE;
        }
        #endregion

        #region Transportadora
        protected void Transp_TxtCEP_Success(object args)
        {

            var ViaCEP = (ViaCEP)args;

            //Transp_TxtLogradouro.Text = ViaCEP.logradouro.ToStringOrNull();
            //Transp_TxtBairro.Text = ViaCEP.bairro.ToStringOrNull();
            Transp_TxtUF.SelectedValue = HelpParametros.Parametros.Estado.FirstOrDefault(c => c.UF == ViaCEP.UF.ToStringOrNull())?.EstadoID.ToStringOrNull();
            Transp_ViewPesquisaMunicipio.Value = ViaCEP.MunicipioID.ToStringOrNull();
            Transp_ViewPesquisaMunicipio.Text = ViaCEP.municipio.ToStringOrNull();
            ViewModel.transp_cMun = ViaCEP.ibge.ToIntOrNull();

            //Transp_TxtNumero.Focus();

        }
        protected void Transp_TxtCEP_Error(object args)
        {

            App.JSRuntime.InvokeVoidAsync("alert", args.ToString());

            //Transp_TxtLogradouro.Text = null;
            //Transp_TxtBairro.Text = null;
            //Transp_ViewPesquisaMunicipio.Clear();

            //Transp_TxtCEP.Focus();

        }
        protected void Transp_TxtUF_Change(object args)
        {

            var Predicate = "EstadoID == @0";

            Transp_ViewPesquisaMunicipio.Where.Remove(Transp_ViewPesquisaMunicipio.Where.FirstOrDefault(c => c.Predicate == Predicate));
            Transp_ViewPesquisaMunicipio.AddWhere(Predicate, ((ChangeEventArgs)args).Value.ToString());

            Transp_ViewPesquisaMunicipio.Clear();

        }
        protected void Transp_ViewPesquisaMunicipio_Change(object args)
        {
            ViewModel.transp_cMun = ((Municipio)args)?.IBGE;
        }
        #endregion

    }
}