using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Aplicativo.View.Pages.Comercial.Vendas;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Faturamento
{
    public partial class ViewFaturamentoPage : ComponentBase
    {

        public PedidoVenda ViewModel = new PedidoVenda();

        [Parameter] public ListItemViewLayout<PedidoVenda> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public ViewPedidoVendaAndamento ViewPedidoVendaAndamento { get; set; }

        public TabSet TabSet { get; set; }

        //public SfGrid<PedidoVenda> GridViewPedidoVenda { get; set; }
        public List<PedidoVenda> ListPedidoVenda { get; set; } = new List<PedidoVenda>();

        public SfGrid<NotaFiscal> GridViewNotaFiscal { get; set; }
        public List<NotaFiscal> ListNotaFiscal { get; set; } = new List<NotaFiscal>();

        #endregion

        public async Task Page_Load(object args)
        {

            var Items = ((IEnumerable)args).Cast<int?>().ToList();

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddInclude("Cliente");
            Query.AddInclude("PedidoVendaItem");
            Query.AddInclude("PedidoVendaItem.Produto");
            Query.AddInclude("PedidoVendaItem.PedidoVendaItemConferenciaItem");
            Query.AddInclude("PedidoVendaItem.PedidoVendaItemConferenciaItem.ConferenciaItem");
            Query.AddInclude("PedidoVendaItem.PedidoVendaItemConferenciaItem.ConferenciaItem.EstoqueMovimentoItem");
            Query.AddInclude("PedidoVendaItem.PedidoVendaItemConferenciaItem.ConferenciaItem.EstoqueMovimentoItem.EstoqueMovimentoItemEntrada");

            Query.AddInclude("PedidoVendaItem.Produto.Tributacao");
            Query.AddInclude("PedidoVendaItem.Produto.Tributacao.TributacaoOperacao");

            Query.AddInclude("PedidoVendaItem.PedidoVendaItemNotaFiscalItem");
            Query.AddInclude("PedidoVendaItem.PedidoVendaItemNotaFiscalItem.NotaFiscalItem");
            Query.AddInclude("PedidoVendaPagamento");
            Query.AddInclude("PedidoVendaPagamento.Titulo");

            Query.AddWhere("PedidoVendaID IN (" + string.Join(",", Items.ToArray()) + ")");

            ListPedidoVenda = await Query.ToList();

            foreach (var PedidoVenda in ListPedidoVenda)
            {

                foreach (var PedidoVendaItem in PedidoVenda.PedidoVendaItem)
                {

                    var QuantidadeFaturado = PedidoVendaItem?.PedidoVendaItemNotaFiscalItem?.Sum(c => c.NotaFiscalItem.qCom ?? 0) ?? 0;

                    if (QuantidadeFaturado < (PedidoVendaItem.Quantidade ?? 0))
                    {

                        var CFOP = PedidoVendaItem.Produto?.Tributacao?.TributacaoOperacao?.FirstOrDefault(c => c.OperacaoID == PedidoVendaItem.OperacaoID);

                        var NotaFiscalItem = new NotaFiscalItem()
                        {
                            cProd = PedidoVendaItem.ProdutoID.ToStringOrNull(),
                            xProd = PedidoVendaItem.Produto.Descricao,
                            qCom = PedidoVendaItem.Quantidade - QuantidadeFaturado,
                            vUnCom = PedidoVendaItem.vTotal / PedidoVendaItem.Quantidade,

                            orig = PedidoVendaItem.Produto.Origem,

                            Codigo_CFOP = CFOP?.Codigo_CFOP_Estadual,

                            Codigo_NCM = PedidoVendaItem.Produto.Codigo_NCM,
                            Codigo_CEST = PedidoVendaItem.Produto.Codigo_CEST,

                            Codigo_CST = PedidoVendaItem.Produto.Tributacao.Codigo_CST,
                            Codigo_CSOSN = PedidoVendaItem.Produto.Tributacao.Codigo_CSOSN,
                            Codigo_IPI = PedidoVendaItem.Produto.Tributacao.Codigo_IPI,
                            Codigo_PIS = PedidoVendaItem.Produto.Tributacao.Codigo_PIS,
                            Codigo_COFINS = PedidoVendaItem.Produto.Tributacao.Codigo_COFINS,

                            pICMS = PedidoVendaItem.Produto.Tributacao.Aliq_ICMS ?? 0,
                            pIPI = PedidoVendaItem.Produto.Tributacao.Aliq_IPI ?? 0,
                            pPIS = PedidoVendaItem.Produto.Tributacao.Aliq_PIS ?? 0,
                            pCOFINS = PedidoVendaItem.Produto.Tributacao.Aliq_COFINS ?? 0,

                        };

                        NotaFiscalItem.vProd = (NotaFiscalItem.vUnCom - NotaFiscalItem.vDesc) * NotaFiscalItem.qCom;
                        NotaFiscalItem.vBC = NotaFiscalItem.vProd;

                        NotaFiscalItem.vICMS = (NotaFiscalItem.pICMS / 100) * NotaFiscalItem.vBC;
                        NotaFiscalItem.vIPI = (NotaFiscalItem.pIPI / 100) * NotaFiscalItem.vBC;
                        NotaFiscalItem.vPIS = (NotaFiscalItem.pPIS / 100) * NotaFiscalItem.vBC;
                        NotaFiscalItem.vCOFINS = (NotaFiscalItem.pCOFINS / 100) * NotaFiscalItem.vBC;

                        NotaFiscalItem.PedidoVendaItemNotaFiscalItem.Add(new PedidoVendaItemNotaFiscalItem()
                        {
                            PedidoVendaItemID = PedidoVendaItem.PedidoVendaItemID,
                        });

                        PedidoVendaItem.PedidoVendaItemNotaFiscalItem.Add(
                            new PedidoVendaItemNotaFiscalItem() { 
                                NotaFiscalItem = NotaFiscalItem,
                            }
                        );
                    }
                }
            }

            CarregarNotasFiscais();

        }

        private void CarregarNotasFiscais()
        {

            ListNotaFiscal.Clear();

            foreach (var PedidoVenda in ListPedidoVenda)
            {
                foreach(var PedidoVendaItem in PedidoVenda.PedidoVendaItem)
                {

                    var NotaFiscalItem = PedidoVendaItem.PedidoVendaItemNotaFiscalItem.Where(c => c.NotaFiscalItem.NotaFiscalItemID == null);


                    foreach(var Item in NotaFiscalItem)
                    {

                        var vDesc = PedidoVendaItem.PedidoVendaItemNotaFiscalItem.Where(c => c.NotaFiscalItem.NotaFiscalItemID == null).Sum(c => c.NotaFiscalItem.vDesc);
                        var vProd = PedidoVendaItem.PedidoVendaItemNotaFiscalItem.Where(c => c.NotaFiscalItem.NotaFiscalItemID == null).Sum(c => c.NotaFiscalItem.vProd);
                        
                        var NotaFiscal = ListNotaFiscal.FirstOrDefault(c => c.cNF.ToIntOrNull() == PedidoVenda.PedidoVendaID);

                        if (NotaFiscal == null)
                        {
                            ListNotaFiscal.Add(new NotaFiscal() { 

                                cNF = PedidoVenda.PedidoVendaID.ToStringOrNull(),

                                tpNF = 1,
                                
                                CNPJCPF = HelpParametros.Parametros.EmpresaLogada.CNPJ,
                                xFant = HelpParametros.Parametros.EmpresaLogada.NomeFantasia,
                                xNome = HelpParametros.Parametros.EmpresaLogada.RazaoSocial,

                                dest_CNPJCPF = PedidoVenda.Cliente.CNPJ,
                                dest_xNome = PedidoVenda.Cliente.NomeFantasia,

                                vDesc = vDesc,
                                vProd = vProd,

                                NotaFiscalItem = new List<NotaFiscalItem>() { Item.NotaFiscalItem },

                            });
                        }
                        else
                        {
                            NotaFiscal.vDesc += vDesc;
                            NotaFiscal.vProd += vProd;
                            NotaFiscal.NotaFiscalItem.Add(Item.NotaFiscalItem);
                        }

                    }

                }
            }

            GridViewNotaFiscal?.Refresh();

        }

        protected async Task BtnFaturar_Click()
        {

            ViewPedidoVendaAndamento.IsFaturado = true;

            ViewPedidoVendaAndamento.PedidoVendaID = ListPedidoVenda.Select(c => c.PedidoVendaID).ToList();

            await ViewPedidoVendaAndamento.EditItemViewLayout.Show(null);

        }

        public async Task ViewPedidoVendaAndamento_Confirm()
        {

            var HelpUpdate = new HelpUpdate();


            //foreach (var item in ListNotaFiscal)
            //{
            //    item.NotaFiscalItem = null;
            //}

            HelpUpdate.AddRange(ListNotaFiscal);

            var EstoqueMovimento = new EstoqueMovimento()
            {
                EstoqueMovimentoTipoID = EstoqueMovimentoTipo.Saida,
                Data = DateTime.Now,
                FuncionarioID = HelpParametros.Parametros.UsuarioLogado.FuncionarioID,
            };

            foreach (var item in ListPedidoVenda)
            {

                foreach (var Titulo in item.PedidoVendaPagamento.Select(c => c.Titulo))
                {
                    Titulo.Ativo = true;
                    HelpUpdate.Add(Titulo);
                }



                foreach (var PedidoVendaItem in item.PedidoVendaItem)
                {


                    if (PedidoVendaItem.PedidoVendaItemConferenciaItem.Count == 0)
                    {

                        var QueryMovimentoItem = new HelpQuery<EstoqueMovimentoItem>();

                        QueryMovimentoItem.AddInclude("EstoqueMovimentoItemEntrada");

                        QueryMovimentoItem.AddWhere("ProdutoID == @0", PedidoVendaItem.ProdutoID);

                        var EstoqueMovimentoItem = await QueryMovimentoItem.ToList();

                        for(var i = 1; i <= PedidoVendaItem.Quantidade; i++)
                        {

                            var Entradas = EstoqueMovimentoItem.Where(c => c?.EstoqueMovimentoItemEntrada?.Saldo > 0).Select(c => c?.EstoqueMovimentoItemEntrada);

                            foreach (var Entrada in Entradas)
                            {

                                var ConferenciaItem = PedidoVendaItem.PedidoVendaItemConferenciaItem.FirstOrDefault(c => c.ConferenciaItem.EstoqueMovimentoItem.EstoqueMovimentoItemEntradaID == Entrada.EstoqueMovimentoItemEntradaID);

                                if (ConferenciaItem == null)
                                {
                                    PedidoVendaItem.PedidoVendaItemConferenciaItem.Add(new PedidoVendaItemConferenciaItem()
                                    {
                                        ConferenciaItem = new ConferenciaItem()
                                        {

                                            Quantidade = 1,

                                            EstoqueMovimentoItem = new EstoqueMovimentoItem()
                                            {
                                                EstoqueMovimentoItemEntrada = Entrada,
                                            }
                                        }
                                    });
                                }
                                else
                                {
                                    ConferenciaItem.ConferenciaItem.EstoqueMovimentoItem.Quantidade++;
                                }

                                if (Entrada.Saldo <= 0)
                                {
                                    throw new Exception(PedidoVendaItem.Produto.Descricao + " não possui estoque suficiente!");
                                }

                                Entrada.Saldo--;

                            }
                            
                        }

                    }

                    foreach (var ConferenciaItem in PedidoVendaItem.PedidoVendaItemConferenciaItem)
                    {

                        var HelpQuery = new HelpQuery<EstoqueMovimentoItemEntrada>();

                        HelpQuery.AddWhere("EstoqueMovimentoItemEntradaID == @0", ConferenciaItem.ConferenciaItem.EstoqueMovimentoItem.EstoqueMovimentoItemEntrada.EstoqueMovimentoItemEntradaID);

                        var EstoqueMovimentoItemEntrada = await HelpQuery.FirstOrDefault(); //ConferenciaItem.ConferenciaItem.EstoqueMovimentoItem.EstoqueMovimentoItemEntrada;

                        await App.JSRuntime.InvokeVoidAsync("console.log", EstoqueMovimentoItemEntrada);

                        var NotaFiscamItem = PedidoVendaItem.PedidoVendaItemNotaFiscalItem.FirstOrDefault(c => c.NotaFiscalItem.NotaFiscalItemID == null);

                        var EstoqueMovimentoItem = new EstoqueMovimentoItem()
                        {
                            ProdutoID = PedidoVendaItem.ProdutoID,
                            Quantidade = ConferenciaItem.ConferenciaItem.Quantidade,
                            NotaFiscalItem = NotaFiscamItem.NotaFiscalItem,
                            EstoqueMovimentoItemSaida = new EstoqueMovimentoItemSaida()
                            {
                                EstoqueMovimentoItemEntradaID = EstoqueMovimentoItemEntrada.EstoqueMovimentoItemEntradaID,
                            }
                        };

                        EstoqueMovimento.EstoqueMovimentoItem.Add(EstoqueMovimentoItem);

                        EstoqueMovimentoItemEntrada.Saldo -= ConferenciaItem.ConferenciaItem.Quantidade;

                        if (EstoqueMovimentoItemEntrada.Saldo < 0)
                        {

                            if (ViewPedidoVendaAndamento != null)
                            {
                                await ViewPedidoVendaAndamento?.EditItemViewLayout?.ViewModal?.Hide();
                            }
                            
                            throw new Exception(PedidoVendaItem.Produto.Descricao + " não possui estoque suficiente!");

                        }

                        HelpUpdate.Add(EstoqueMovimentoItemEntrada);

                    }

                }


                item.Cliente = null;
                item.Vendedor = null;
                item.Transportadora = null;

                //if (item.Finalizado == null)
                //{
                //    item.Finalizado = DateTime.Now;
                //}

                if (item.PedidoVendaItem.Sum(c => c.PedidoVendaItemNotaFiscalItem.Sum(c => c.NotaFiscalItem.qCom ?? 0)) == item.PedidoVendaItem.Sum(c => c.Quantidade))
                {
                    item.Faturado = DateTime.Now;
                }

                item.PedidoVendaItem = null;
                item.PedidoVendaPagamento = null;

            }

            HelpUpdate.Add(EstoqueMovimento);
            HelpUpdate.AddRange(ListPedidoVenda);
           

            await HelpUpdate.SaveChanges();

            if (EditItemViewLayout != null)
            {
                await EditItemViewLayout?.ViewModal?.Hide();
            }
            
            if (ViewPedidoVendaAndamento != null)
            {
                await ViewPedidoVendaAndamento?.EditItemViewLayout?.ViewModal?.Hide();
            }
            

        }

        protected async Task ViewPedidoVendaAndamento_Finally()
        {

            await ListView.ListViewBtnPesquisa.BtnPesquisar_Click();

        }

    }

}