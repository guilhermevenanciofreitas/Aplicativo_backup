using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Produtos
{
    public class ViewProdutoFornecedorPage : HelpComponent
    {

        public ListItemViewLayout<ProdutoFornecedor> ListItemViewLayout { get; set; }
        public EditItemViewLayout<ProdutoFornecedor> EditItemViewLayout { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtFornecedor { get; set; }
        public DropDownList DplUnidadeMedida { get; set; }
        public NumericBox TxtContem { get; set; }
        public NumericBox TxtPreco { get; set; }
        public NumericBox TxtTotal { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

            }
        }

        protected async void ViewLayout_Limpar()
        {
            await EditItemViewLayout.LimparCampos(this);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((ProdutoFornecedor)args);
        }

        protected async void ViewLayout_Carregar(object args)
        {

            EditItemViewLayout.ViewModel = (ProdutoFornecedor)args;

            TxtCodigo.Text = EditItemViewLayout.ViewModel.CodigoFornecedor.ToStringOrNull();
            TxtFornecedor.Text = EditItemViewLayout.ViewModel.Fornecedor?.NomeFantasia.ToStringOrNull();

            DplUnidadeMedida.SelectedValue = EditItemViewLayout.ViewModel.UnidadeMedidaID.ToStringOrNull();
            //await TxtContem.SetValue(EditItemViewLayout.ViewModel.Contem);
            //await TxtPreco.SetValue(EditItemViewLayout.ViewModel.Preco);
            //await TxtTotal.SetValue(EditItemViewLayout.ViewModel.Contem * EditItemViewLayout.ViewModel.Preco);


        }

        protected async void ViewLayout_Salvar()
        {

            EditItemViewLayout.ViewModel.CodigoFornecedor = TxtCodigo.Text.ToStringOrNull();

            EditItemViewLayout.ViewModel.UnidadeMedidaID = DplUnidadeMedida.SelectedValue.ToIntOrNull();
            //EditItemViewLayout.ViewModel.Contem = await TxtContem.GetValue();
            //EditItemViewLayout.ViewModel.Preco = await TxtPreco.GetValue();


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
            }
            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Delete(object args)
        {
            foreach(var item in (List<ProdutoFornecedor>)args) ListItemViewLayout.ListItemView.Remove(item);
            EditItemViewLayout.ViewModal.Hide();
        }

    }
}