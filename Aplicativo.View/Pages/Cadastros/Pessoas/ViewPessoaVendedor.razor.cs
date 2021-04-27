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

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public class ViewPessoaVendedorPage : HelpComponent
    {

        public ListItemViewLayout<PessoaVendedor> ListItemViewLayout { get; set; }
        public EditItemViewLayout<PessoaVendedor> EditItemViewLayout { get; set; }

        public TextBox TxtVendedor { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

            }
        }

        protected void ViewLayout_Limpar()
        {
            EditItemViewLayout.LimparCampos(this);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((PessoaVendedor)args);
        }

        protected void ViewLayout_Carregar(object args)
        {

            EditItemViewLayout.ViewModel = (PessoaVendedor)args;

            TxtVendedor.Text = EditItemViewLayout.ViewModel.Vendedor.NomeFantasia.ToStringOrNull();
           

        }

        protected void ViewLayout_Salvar()
        {

            EditItemViewLayout.ViewModel.VendedorID = null; //TxtCodigo.Text.ToStringOrNull();


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
            }
            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Delete(object args)
        {
            foreach(var item in (List<PessoaVendedor>)args) ListItemViewLayout.ListItemView.Remove(item);
            EditItemViewLayout.ViewModal.Hide();
        }

    }
}