using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
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

                    var QuantidadeFaturado = PedidoVendaItem.PedidoVendaItemNotaFiscalItem.Sum(c => c.NotaFiscalItem.qCom);

                    if (QuantidadeFaturado < PedidoVendaItem.Quantidade)
                    {

                        var NotaFiscalItem = new NotaFiscalItem()
                        {
                            cProd = PedidoVendaItem.ProdutoID.ToStringOrNull(),
                            xProd = PedidoVendaItem.Produto.Descricao,
                            qCom = PedidoVendaItem.Quantidade - QuantidadeFaturado,
                            vUnCom = PedidoVendaItem.vTotal / PedidoVendaItem.Quantidade,

                            orig = PedidoVendaItem.Produto.Origem,
                            Codigo_CFOP = "5.102",
                            Codigo_NCM = PedidoVendaItem.Produto.Codigo_NCM,
                            Codigo_CEST = PedidoVendaItem.Produto.Codigo_CEST,

                        };

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

                    var NaoFaturado = PedidoVendaItem.PedidoVendaItemNotaFiscalItem.Where(c => c.NotaFiscalItem.NotaFiscalItemID == null);

                    foreach(var Item in NaoFaturado)
                    {

                        var vTotal = PedidoVendaItem.PedidoVendaItemNotaFiscalItem.Where(c => c.NotaFiscalItem.NotaFiscalItemID == null).Sum(c => c.NotaFiscalItem.qCom * c.NotaFiscalItem.vUnCom);

                        var NotaFiscal = ListNotaFiscal.FirstOrDefault(c => c.cNF.ToIntOrNull() == PedidoVenda.PedidoVendaID);

                        if (NotaFiscal == null)
                        {
                            ListNotaFiscal.Add(new NotaFiscal() { 
                                cNF = PedidoVenda.PedidoVendaID.ToStringOrNull(), 
                                dest_xNome = PedidoVenda.Cliente.NomeFantasia,
                                vProd = vTotal,
                                NotaFiscalItem = new List<NotaFiscalItem>() { Item.NotaFiscalItem },
                            });
                        }
                        else
                        {
                            NotaFiscal.vProd += vTotal;
                            NotaFiscal.NotaFiscalItem.Add(Item.NotaFiscalItem);
                        }

                    }

                }
            }

            GridViewNotaFiscal.Refresh();

        }

        protected async Task BtnFaturar_Click()
        {

            var HelpUpdate = new HelpUpdate();


            HelpUpdate.AddRange(ListNotaFiscal);


            foreach (var item in ListPedidoVenda)
            {

                foreach(var Titulo in item.PedidoVendaPagamento.Select(c => c.Titulo))
                {
                    Titulo.Ativo = true;
                    HelpUpdate.Add(Titulo);
                }


                item.Cliente = null;
                item.Vendedor = null;
                item.Transportadora = null;

                if (item.Finalizado == null)
                {
                    item.Finalizado = DateTime.Now;
                }

                if (item.PedidoVendaItem.Sum(c => c.PedidoVendaItemNotaFiscalItem.Sum(c => c.NotaFiscalItem.qCom ?? 0)) == item.PedidoVendaItem.Sum(c => c.Quantidade))
                {
                    item.Faturamento = DateTime.Now;
                }

                item.PedidoVendaItem = null;
                item.PedidoVendaPagamento = null;

            }

            HelpUpdate.AddRange(ListPedidoVenda);


            await HelpUpdate.SaveChanges();

            await EditItemViewLayout.ViewModal.Hide();

            await ListView.ListViewBtnPesquisa.BtnPesquisar_Click();

        }

    }

}