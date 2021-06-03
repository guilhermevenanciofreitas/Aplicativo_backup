using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Produtos
{
    public class ViewProdutoFornecedorPage : ComponentBase
    {

        public ProdutoFornecedor ViewModel { get; set; } = new ProdutoFornecedor();

        public ListItemViewLayout<ProdutoFornecedor> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TextBox TxtCodigo { get; set; }
        public ViewPesquisa<Pessoa> ViewPesquisaFornecedor { get; set; }
        public DropDownList DplUnidadeMedida { get; set; }
        public NumericBox TxtContem { get; set; }
        public NumericBox TxtPreco { get; set; }
        public NumericBox TxtTotal { get; set; }
        #endregion

        #region ListView
        protected void Page_Load()
        {
            ViewPesquisaFornecedor.AddWhere("IsFornecedor == @0", true);
            ViewPesquisaFornecedor.AddWhere("Ativo == @0", true);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void BtnLimpar_Click()
        {

            ViewModel = new ProdutoFornecedor();

            EditItemViewLayout.LimparCampos(this);

            TxtCodigo.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            BtnLimpar_Click();

            if (args == null) return;

            ViewModel = (ProdutoFornecedor)args;

            TxtCodigo.Text = ViewModel.CodigoFornecedor.ToStringOrNull();
            ViewPesquisaFornecedor.Value = ViewModel.FornecedorID.ToStringOrNull();
            ViewPesquisaFornecedor.Text = ViewModel.Fornecedor?.NomeFantasia.ToStringOrNull();
            TxtContem.Value = ViewModel.Contem ?? 0;
            TxtPreco.Value = ViewModel.Preco ?? 0;

            DplUnidadeMedida.SelectedValue = ViewModel.UnidadeMedidaID.ToStringOrNull();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            ViewModel.CodigoFornecedor = TxtCodigo.Text.ToStringOrNull();
            ViewModel.FornecedorID = ViewPesquisaFornecedor.Value.ToIntOrNull();
            ViewModel.Fornecedor = new Pessoa() { NomeFantasia = ViewPesquisaFornecedor.Text };
            ViewModel.UnidadeMedidaID = DplUnidadeMedida.SelectedValue.ToIntOrNull();
            ViewModel.Contem = TxtContem.Value;
            ViewModel.Preco = TxtPreco.Value;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<ProdutoFornecedor>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<ProdutoFornecedor>().ToList());

        }

        public void Excluir(List<ProdutoFornecedor> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        protected void TxtContem_KeyUp()
        {
            Calcular();
        }

        protected void TxtPreco_KeyUp()
        {
            Calcular();
        }

        private void Calcular()
        {
            TxtTotal.Value = TxtContem.Value * TxtPreco.Value;
        }

        #endregion

    }
}