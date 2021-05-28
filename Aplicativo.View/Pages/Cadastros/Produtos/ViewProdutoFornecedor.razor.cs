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
        public TextBox TxtFornecedor { get; set; }
        public DropDownList DplUnidadeMedida { get; set; }
        public NumericBox TxtContem { get; set; }
        public NumericBox TxtPreco { get; set; }
        public NumericBox TxtTotal { get; set; }
        #endregion

        #region ListView
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
        }

        protected async Task ViewLayout_Carregar(object args)
        {

            BtnLimpar_Click();

            await EditItemViewLayout.Show(args);

            if (args == null) return;

            ViewModel = (ProdutoFornecedor)args;

            TxtCodigo.Text = ViewModel.CodigoFornecedor.ToStringOrNull();
            TxtFornecedor.Text = ViewModel.Fornecedor?.NomeFantasia.ToStringOrNull();
            TxtContem.Value = ViewModel.Contem ?? 0;
            TxtPreco.Value = ViewModel.Preco ?? 0;

            DplUnidadeMedida.SelectedValue = ViewModel.UnidadeMedidaID.ToStringOrNull();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            ViewModel.CodigoFornecedor = TxtCodigo.Text.ToStringOrNull();
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

        #endregion


        //public ListItemViewLayout<ProdutoFornecedor> ListItemViewLayout { get; set; }
        //public EditItemViewLayout<ProdutoFornecedor> EditItemViewLayout { get; set; }

        //public TextBox TxtCodigo { get; set; }
        //public TextBox TxtFornecedor { get; set; }
        //public DropDownList DplUnidadeMedida { get; set; }
        //public NumericBox TxtContem { get; set; }
        //public NumericBox TxtPreco { get; set; }
        //public NumericBox TxtTotal { get; set; }


        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await base.OnAfterRenderAsync(firstRender);

        //    if (firstRender)
        //    {

        //    }
        //}

        //protected void ViewLayout_Limpar()
        //{
        //    EditItemViewLayout.LimparCampos(this);
        //}

        //protected async Task ViewLayout_ItemView(object args)
        //{
        //    await EditItemViewLayout.Carregar((ProdutoFornecedor)args);
        //}

        //protected async void ViewLayout_Carregar(object args)
        //{

        //    EditItemViewLayout.ViewModel = (ProdutoFornecedor)args;

        //    TxtCodigo.Text = EditItemViewLayout.ViewModel.CodigoFornecedor.ToStringOrNull();
        //    TxtFornecedor.Text = EditItemViewLayout.ViewModel.Fornecedor?.NomeFantasia.ToStringOrNull();

        //    DplUnidadeMedida.SelectedValue = EditItemViewLayout.ViewModel.UnidadeMedidaID.ToStringOrNull();
        //    //await TxtContem.SetValue(EditItemViewLayout.ViewModel.Contem);
        //    //await TxtPreco.SetValue(EditItemViewLayout.ViewModel.Preco);
        //    //await TxtTotal.SetValue(EditItemViewLayout.ViewModel.Contem * EditItemViewLayout.ViewModel.Preco);


        //}

        //protected async void ViewLayout_Salvar()
        //{

        //    EditItemViewLayout.ViewModel.CodigoFornecedor = TxtCodigo.Text.ToStringOrNull();

        //    EditItemViewLayout.ViewModel.UnidadeMedidaID = DplUnidadeMedida.SelectedValue.ToIntOrNull();
        //    //EditItemViewLayout.ViewModel.Contem = await TxtContem.GetValue();
        //    //EditItemViewLayout.ViewModel.Preco = await TxtPreco.GetValue();


        //    if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
        //    {
        //        ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
        //    }
        //    EditItemViewLayout.ViewModal.Hide();

        //}

        //protected void ViewLayout_Delete(object args)
        //{
        //    foreach(var item in (List<ProdutoFornecedor>)args) ListItemViewLayout.ListItemView.Remove(item);
        //    EditItemViewLayout.ViewModal.Hide();
        //}

    }
}