using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Aplicativo.View.Pages.Comercial.Vendas;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Conferencias
{
    public partial class ViewConferenciaPage : ComponentBase
    {

        public Conferencia ViewModel = new Conferencia();

        public PedidoVenda PedidoVenda = new PedidoVenda();

        [Parameter] public bool BtnLimpar { get; set; } = true;
        [Parameter] public bool BtnExcluir { get; set; } = true;

        [Parameter] public ListItemViewLayout<PedidoVenda> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public ViewPedidoVendaAndamento ViewPedidoVendaAndamento { get; set; }

        public TabSet TabSet { get; set; }

        public NumericBox TxtQuantidade { get; set; }
        public TextBox TxtCodigoBarras { get; set; }

        public List<EstoqueMovimentoItem> EstoqueMovimentoItem { get; set; } = new List<EstoqueMovimentoItem>();

        public SfGrid<PedidoVendaItem> GridViewPedidoVendaItem { get; set; }
        public List<PedidoVendaItem> ListPedidoVendaItem { get; set; } = new List<PedidoVendaItem>();

        public SfGrid<ConferenciaItem> GridViewConferenciaItem { get; set; }
        public List<ConferenciaItem> ListConferenciaItem { get; set; } = new List<ConferenciaItem>();

        public SfGrid<PedidoVendaAndamento> GridViewAndamento { get; set; }
        public List<PedidoVendaAndamento> ListAndamento { get; set; } = new List<PedidoVendaAndamento>();

        #endregion

        public async Task Page_Load(object args)
        {

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddInclude("PedidoVendaItem");
            Query.AddInclude("PedidoVendaItem.Produto");
            Query.AddInclude("PedidoVendaAndamento");
            Query.AddInclude("PedidoVendaAndamento.PedidoVendaStatus");

            Query.AddWhere("PedidoVendaID == @0", ((PedidoVenda)args).PedidoVendaID);
            
            PedidoVenda = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            var Produtos = PedidoVenda.PedidoVendaItem.Select(c => (int)c.ProdutoID);

            var QueryMovimentoItem = new HelpQuery<EstoqueMovimentoItem>();

            QueryMovimentoItem.AddInclude("EstoqueMovimentoItemEntrada");


            if (Produtos.Count() == 0) 
            {
                await App.JSRuntime.InvokeVoidAsync("alert", "Pedido não possui itens para conferência");
                await EditItemViewLayout.ViewModal.Hide();
                return;
            }

            QueryMovimentoItem.AddWhere("ProdutoID IN (" + string.Join(",", Produtos.ToArray()) + ")");

            EstoqueMovimentoItem = await QueryMovimentoItem.ToList();
            

            ListPedidoVendaItem.Clear();
            ListPedidoVendaItem.AddRange(PedidoVenda.PedidoVendaItem.ToList());;
            GridViewPedidoVendaItem.Refresh();

            ListAndamento.Clear();
            ListAndamento.AddRange(PedidoVenda.PedidoVendaAndamento.ToList());
            GridViewAndamento.Refresh();


        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            TxtQuantidade.Value = 1;

            ListPedidoVendaItem.Clear();
            GridViewPedidoVendaItem.Refresh();

            ListConferenciaItem.Clear();
            GridViewConferenciaItem.Refresh();


            ListAndamento.Clear();
            GridViewAndamento.Refresh();


            await TabSet.Active("Principal");

            TxtCodigoBarras.Focus();

        }

        protected async Task BtnFinalizar_Click()
        {

            if (ListPedidoVendaItem.Any())
            {
                await App.JSRuntime.InvokeVoidAsync("alert", "Ainda existem itens para conferir!");
                return;
            }

            ViewPedidoVendaAndamento.IsConferido = true;

            ViewPedidoVendaAndamento.PedidoVendaID = new List<int?>() { PedidoVenda.PedidoVendaID };

            await ViewPedidoVendaAndamento.EditItemViewLayout.Show(null);

            //await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPedidoVendaAndamento_Confirm()
        {

            ViewModel.Data = DateTime.Now;
            ViewModel.FuncionarioID = HelpParametros.Parametros.UsuarioLogado.FuncionarioID;

            ViewModel.ConferenciaItem = ListConferenciaItem.ToList();

            foreach (var item in ViewModel.ConferenciaItem)
            {
                item.Produto = null;
            }

           // PedidoVenda.Conferido = DateTime.Now;
            //PedidoVenda.PedidoVendaItem = null;

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);
            //HelpUpdate.Add(PedidoVenda);

            await HelpUpdate.SaveChanges();

            await ViewPedidoVendaAndamento.EditItemViewLayout.ViewModal.Hide();
            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPedidoVendaAndamento_Finally()
        {

            await ListView.ListViewBtnPesquisa.BtnPesquisar_Click();

        }

        protected void BtnAdicionar_Click()
        {

            var MovimentoItem = EstoqueMovimentoItem.FirstOrDefault(c => c.EstoqueMovimentoItemEntrada.CodigoBarra == TxtCodigoBarras.Text.ToLongOrNull());

            if (MovimentoItem == null)
            {
                App.JSRuntime.InvokeVoidAsync("alert", "Item não encontrado no pedido!");
                TxtCodigoBarras.Text = null;
                TxtCodigoBarras.Focus();
                return;
            }

            var ConferenciaItem = ListConferenciaItem.FirstOrDefault(c => c.ProdutoID == MovimentoItem.ProdutoID);

            var PedidoVendaItem = ListPedidoVendaItem.FirstOrDefault(c => c.ProdutoID == MovimentoItem.ProdutoID);

            if (PedidoVendaItem == null)
            {
                App.JSRuntime.InvokeVoidAsync("alert", "Item já conferido para o pedido!");
                TxtCodigoBarras.Text = null;
                TxtCodigoBarras.Focus();
                return;
            }

            if (PedidoVendaItem.Quantidade < TxtQuantidade.Value)
            {
                App.JSRuntime.InvokeVoidAsync("alert", "Quantidade superior ao pedido!");
                TxtCodigoBarras.Text = null;
                TxtCodigoBarras.Focus();
                return;
            }

            PedidoVendaItem.Quantidade -= TxtQuantidade.Value;

            if (PedidoVendaItem.Quantidade == 0)
            {
                ListPedidoVendaItem.Remove(PedidoVendaItem);
            }

            if (ConferenciaItem == null)
            {

                var Item = new ConferenciaItem()
                {
                    EstoqueMovimentoItemID = MovimentoItem.EstoqueMovimentoItemID,
                    ProdutoID = PedidoVendaItem.ProdutoID,
                    Produto = PedidoVendaItem.Produto,
                    Quantidade = TxtQuantidade.Value,
                };

                Item.PedidoVendaItemConferenciaItem.Add(new PedidoVendaItemConferenciaItem()
                {
                    PedidoVendaItemID = PedidoVendaItem.PedidoVendaItemID,
                });

                ListConferenciaItem.Add(Item);

            }
            else
            {
                ConferenciaItem.Quantidade += TxtQuantidade.Value;
            }


            GridViewPedidoVendaItem.Refresh();
            GridViewConferenciaItem.Refresh();
            StateHasChanged();

            TxtQuantidade.Value = 1;

            TxtCodigoBarras.Text = null;
            TxtCodigoBarras.Focus();

        }

    }
}