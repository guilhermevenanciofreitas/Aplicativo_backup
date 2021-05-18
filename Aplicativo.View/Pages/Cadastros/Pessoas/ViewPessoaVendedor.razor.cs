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

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public class ViewPessoaVendedorPage : ComponentBase
    {


        public PessoaVendedor ViewModel { get; set; } = new PessoaVendedor();

        public ListItemViewLayout ListItemViewLayout { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        public TextBox TxtVendedor { get; set; }


        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void ViewLayout_Limpar()
        {

            ViewModel = new PessoaVendedor();

            EditItemViewLayout.LimparCampos(this);

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            ViewLayout_Limpar();

            await EditItemViewLayout.Show(args);

            if (args == null) return;

            ViewModel = (PessoaVendedor)args;

            TxtVendedor.Text = ViewModel.Vendedor.NomeFantasia.ToStringOrNull();

        }

        protected void ViewLayout_Salvar()
        {

            var ListItemView = ListItemViewLayout.ListItemView;

            ViewModel.VendedorID = null;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemView.Add(ViewModel);
            }

            ListItemViewLayout.ListItemView = ListItemView;

            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Excluir(object args)
        {

            var ListItemView = ListItemViewLayout.ListItemView;

            foreach (var item in ((IEnumerable)args).Cast<PessoaVendedor>().ToList())
            {
                ListItemView.Remove(item);
            }

            ListItemViewLayout.ListItemView = ListItemView;

            EditItemViewLayout.ViewModal.Hide();

        }

        #endregion

        //public ListItemViewLayout<PessoaVendedor> ListItemViewLayout { get; set; }
        //public EditItemViewLayout<PessoaVendedor> EditItemViewLayout { get; set; }

        //public TextBox TxtVendedor { get; set; }

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
        //    await EditItemViewLayout.Carregar((PessoaVendedor)args);
        //}

        //protected void ViewLayout_Carregar(object args)
        //{

        //    EditItemViewLayout.ViewModel = (PessoaVendedor)args;

        //    TxtVendedor.Text = EditItemViewLayout.ViewModel.Vendedor.NomeFantasia.ToStringOrNull();


        //}

        //protected void ViewLayout_Salvar()
        //{

        //    EditItemViewLayout.ViewModel.VendedorID = null; //TxtCodigo.Text.ToStringOrNull();


        //    if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
        //    {
        //        ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
        //    }
        //    EditItemViewLayout.ViewModal.Hide();

        //}

        //protected void ViewLayout_Delete(object args)
        //{
        //    foreach(var item in (List<PessoaVendedor>)args) ListItemViewLayout.ListItemView.Remove(item);
        //    EditItemViewLayout.ViewModal.Hide();
        //}

    }
}