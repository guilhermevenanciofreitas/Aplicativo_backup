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
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.ContaBancarias
{
    public class ViewContaBancariaFormaPagamentoPage : ComponentBase
    {

        public ContaBancariaFormaPagamento ViewModel { get; set; } = new ContaBancariaFormaPagamento();

        public ListItemViewLayout ListItemViewLayout { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }


        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void ViewLayout_Limpar()
        {
            ViewModel = new ContaBancariaFormaPagamento();
            EditItemViewLayout.LimparCampos(this);
        }

        protected async Task ViewLayout_Carregar(object args)
        {

            ViewLayout_Limpar();

            await EditItemViewLayout.Show(args);

            if (args == null) return;

            ViewModel = (ContaBancariaFormaPagamento)args;

            //TxtSmtp.Text = ViewModel.Smtp.ToStringOrNull();
            

        }

        protected void ViewLayout_Salvar()
        {

            var ListItemView = ListItemViewLayout.ListItemView;

            //ViewModel.Smtp = TxtSmtp.Text.ToStringOrNull();
            
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

            foreach (var item in ((IEnumerable)args).Cast<ContaBancariaFormaPagamento>().ToList())
            {
                ListItemView.Remove(item);
            }

            ListItemViewLayout.ListItemView = ListItemView;

            EditItemViewLayout.ViewModal.Hide();

        }

        #endregion


        //public ListItemViewLayout<ContaBancariaFormaPagamento> ListItemViewLayout { get; set; }
        //public EditItemViewLayout<ContaBancariaFormaPagamento> EditItemViewLayout { get; set; }

        //public TextBox TxtSmtp { get; set; }
        //public TextBox TxtPorta { get; set; }
        //public TextBox TxtEmail { get; set; }
        //public TextBox TxtSenha { get; set; }
        //public TextBox TxtConfirmarSenha { get; set; }

        //public CheckBox ChkSSL { get; set; }


        //protected void ViewLayout_PageLoad()
        //{

        //}

        //protected void ViewLayout_Limpar()
        //{

        //    EditItemViewLayout.LimparCampos(this);

        //    ChkSSL.Checked = true;

        //}

        //protected async Task ViewLayout_ItemView(object args)
        //{
        //    await EditItemViewLayout.Carregar((UsuarioEmail)args);
        //}

        //protected void ViewLayout_Carregar(object args)
        //{

        //    EditItemViewLayout.ViewModel = (ContaBancariaFormaPagamento)args;

        //    //TxtSmtp.Text = EditItemViewLayout.ViewModel.Smtp.ToStringOrNull();
        //    //TxtPorta.Text = EditItemViewLayout.ViewModel.Porta.ToStringOrNull();
        //    //TxtEmail.Text = EditItemViewLayout.ViewModel.Email.ToStringOrNull();
        //    //TxtSenha.Text = EditItemViewLayout.ViewModel.Senha.ToStringOrNull();
        //    //TxtConfirmarSenha.Text = EditItemViewLayout.ViewModel.Senha.ToStringOrNull();
        //    //ChkSSL.Checked = EditItemViewLayout.ViewModel.Ssl.ToBoolean();

        //}

        //protected async Task ViewLayout_Salvar()
        //{

        //    //EditItemViewLayout.ViewModel.Smtp = TxtSmtp.Text.ToStringOrNull();
        //    //EditItemViewLayout.ViewModel.Porta = TxtPorta.Text.ToIntOrNull();
        //    //EditItemViewLayout.ViewModel.Email = TxtEmail.Text.ToStringOrNull();
        //    //EditItemViewLayout.ViewModel.Senha = TxtSenha.Text.ToStringOrNull();
        //    //EditItemViewLayout.ViewModel.Ssl = ChkSSL.Checked;

        //    if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
        //    {
        //        ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
        //    }
        //    EditItemViewLayout.ViewModal.Hide();

        //}

        //protected void ViewLayout_Delete(object args)
        //{
        //    foreach(var item in (List<ContaBancariaFormaPagamento>)args) ListItemViewLayout.ListItemView.Remove(item);
        //    EditItemViewLayout.ViewModal.Hide();
        //}

    }
}