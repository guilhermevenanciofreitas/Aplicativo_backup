using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Vendas
{
    public class ViewPedidoVendaItemPage : ComponentBase
    {

        public PedidoVendaItem ViewModel { get; set; } = new PedidoVendaItem();

        public ListItemViewLayout<PedidoVendaItem> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public ViewPesquisa<Produto> ViewPesquisaProduto { get; set; }
        public NumericBox TxtQuantidade { get; set; }
        public NumericBox TxtPreco { get; set; }
        public NumericBox TxtValorDesconto { get; set; }
        public NumericBox TxtPercentualDesconto { get; set; }
        public NumericBox TxtDescontoTotal { get; set; }
        public NumericBox TxtTotal { get; set; }
        #endregion

        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void ViewLayout_Limpar()
        {

            ViewPesquisaProduto.AddWhere("Ativo == @0", true);

            ViewModel = new PedidoVendaItem();

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaProduto.Clear();

            ViewPesquisaProduto.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            ViewLayout_Limpar();

            if (args == null) return;

            ViewModel = (PedidoVendaItem)args;

            ViewPesquisaProduto.Value = ViewModel.ProdutoID.ToStringOrNull();
            ViewPesquisaProduto.Text = ViewModel.Produto?.Descricao.ToStringOrNull();
            TxtQuantidade.Value = ViewModel.Quantidade ?? 0;
            TxtPreco.Value = ViewModel.vPreco ?? 0;
            TxtPercentualDesconto.Value = ViewModel.pDesconto ?? 0;
            TxtValorDesconto.Value = ViewModel.vDesconto ?? 0;
            TxtDescontoTotal.Value = ViewModel.DescontoTotal ?? 0;
            TxtTotal.Value = ViewModel.vTotal ?? 0;

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            if (ViewPesquisaProduto.Value.ToIntOrNull() == null)
            {
                throw new EmptyException("Informe o produto!", ViewPesquisaProduto.Element);
            }

            ViewModel.ProdutoID = ViewPesquisaProduto.Value.ToIntOrNull();
            ViewModel.Produto = new Produto() { Descricao = ViewPesquisaProduto.Text };
            ViewModel.Quantidade = TxtQuantidade.Value;
            ViewModel.vPreco = TxtPreco.Value;
            ViewModel.pDesconto = TxtPercentualDesconto.Value;
            ViewModel.vDesconto = TxtValorDesconto.Value;
            ViewModel.DescontoTotal = TxtDescontoTotal.Value;
            ViewModel.vTotal = TxtTotal.Value;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<PedidoVendaItem>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<PedidoVendaItem>().ToList());

        }

        public void Excluir(List<PedidoVendaItem> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }



        protected void TxtQuantidade_KeyUp()
        {
            TxtDescontoTotal.Value = TxtValorDesconto.Value * TxtQuantidade.Value;
            CalcularTotal();
        }
        protected void TxtPreco_KeyUp()
        {
            TxtDescontoTotal.Value = TxtValorDesconto.Value * TxtQuantidade.Value;
            TxtPercentualDesconto_KeyUp();
        }
        protected void TxtPercentualDesconto_KeyUp()
        {
            TxtValorDesconto.Value = (TxtPreco.Value / 100) * TxtPercentualDesconto.Value;
            TxtDescontoTotal.Value = TxtValorDesconto.Value * TxtQuantidade.Value;
            CalcularTotal();
        }
        protected void TxtValorDesconto_KeyUp()
        {
            if (TxtPreco.Value == 0)
            {
                TxtPercentualDesconto.Value = 0;
                return;
            }

            TxtPercentualDesconto.Value = (TxtValorDesconto.Value / TxtPreco.Value) * 100;
            TxtPreco_KeyUp();

        }

        protected void TxtDescontoTotal_KeyUp()
        {

            if (TxtQuantidade.Value == 0)
            {
                TxtValorDesconto.Value = 0;
                return;
            }

            if (TxtPreco.Value == 0)
            {
                TxtPercentualDesconto.Value = 0;
                return;
            }

            TxtValorDesconto.Value = TxtDescontoTotal.Value / TxtQuantidade.Value;
            TxtPercentualDesconto.Value = (TxtValorDesconto.Value / TxtPreco.Value) * 100;
            CalcularTotal();

        }


        private void CalcularTotal()
        {
            TxtTotal.Value = (TxtQuantidade.Value * TxtPreco.Value) - TxtDescontoTotal.Value;
        }


        #endregion

    }
}