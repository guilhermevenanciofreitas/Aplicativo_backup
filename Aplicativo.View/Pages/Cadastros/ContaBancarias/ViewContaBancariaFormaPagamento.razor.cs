using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.ContaBancarias
{
    public class ViewContaBancariaFormaPagamentoPage : HelpComponent
    {

        public ListItemViewLayout<ContaBancariaFormaPagamento> ListItemViewLayout { get; set; }
        public EditItemViewLayout<ContaBancariaFormaPagamento> EditItemViewLayout { get; set; }

        public TextBox TxtSmtp { get; set; }
        public TextBox TxtPorta { get; set; }
        public TextBox TxtEmail { get; set; }
        public TextBox TxtSenha { get; set; }
        public TextBox TxtConfirmarSenha { get; set; }

        public CheckBox ChkSSL { get; set; }


        protected void ViewLayout_PageLoad()
        {

        }

        protected void ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            ChkSSL.Checked = true;

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((UsuarioEmail)args);
        }

        protected void ViewLayout_Carregar(object args)
        {

            EditItemViewLayout.ViewModel = (ContaBancariaFormaPagamento)args;

            //TxtSmtp.Text = EditItemViewLayout.ViewModel.Smtp.ToStringOrNull();
            //TxtPorta.Text = EditItemViewLayout.ViewModel.Porta.ToStringOrNull();
            //TxtEmail.Text = EditItemViewLayout.ViewModel.Email.ToStringOrNull();
            //TxtSenha.Text = EditItemViewLayout.ViewModel.Senha.ToStringOrNull();
            //TxtConfirmarSenha.Text = EditItemViewLayout.ViewModel.Senha.ToStringOrNull();
            //ChkSSL.Checked = EditItemViewLayout.ViewModel.Ssl.ToBoolean();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtSmtp.Text))
                await HelpEmptyException.New(JSRuntime, TxtSmtp.Element, "Informe o SMTP!");
            
            if (string.IsNullOrEmpty(TxtPorta.Text))
                await HelpEmptyException.New(JSRuntime, TxtPorta.Element, "Informe a porta!");
           
            if (string.IsNullOrEmpty(TxtEmail.Text))
                await HelpEmptyException.New(JSRuntime, TxtEmail.Element, "Informe o email!");

            if (string.IsNullOrEmpty(TxtSenha.Text))
                await HelpEmptyException.New(JSRuntime, TxtSenha.Element, "Informe a senha!");
            
            if (TxtSenha.Text != TxtConfirmarSenha.Text)
                await HelpEmptyException.New(JSRuntime, TxtConfirmarSenha.Element, "A confirmação da senha está diferente da senha informada!");
            

            //EditItemViewLayout.ViewModel.Smtp = TxtSmtp.Text.ToStringOrNull();
            //EditItemViewLayout.ViewModel.Porta = TxtPorta.Text.ToIntOrNull();
            //EditItemViewLayout.ViewModel.Email = TxtEmail.Text.ToStringOrNull();
            //EditItemViewLayout.ViewModel.Senha = TxtSenha.Text.ToStringOrNull();
            //EditItemViewLayout.ViewModel.Ssl = ChkSSL.Checked;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
            }
            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Delete(object args)
        {
            foreach(var item in (List<ContaBancariaFormaPagamento>)args) ListItemViewLayout.ListItemView.Remove(item);
            EditItemViewLayout.ViewModal.Hide();
        }

    }
}