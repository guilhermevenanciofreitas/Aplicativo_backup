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

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public class ViewUsuarioEmailPage : HelpComponent
    {

        public ListItemViewLayout<UsuarioEmail> ListItemViewLayout { get; set; }
        public EditItemViewLayout<UsuarioEmail> EditItemViewLayout { get; set; }

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

            EditItemViewLayout.ViewModel = (UsuarioEmail)args;

            TxtSmtp.Text = EditItemViewLayout.ViewModel.Smtp.ToStringOrNull();
            TxtPorta.Text = EditItemViewLayout.ViewModel.Porta.ToStringOrNull();
            TxtEmail.Text = EditItemViewLayout.ViewModel.Email.ToStringOrNull();
            TxtSenha.Text = EditItemViewLayout.ViewModel.Senha.ToStringOrNull();
            TxtConfirmarSenha.Text = EditItemViewLayout.ViewModel.Senha.ToStringOrNull();
            ChkSSL.Checked = EditItemViewLayout.ViewModel.Ssl.ToBoolean();

        }

        protected void ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtSmtp.Text))
                throw new EmptyException("Informe o SMTP!", TxtSmtp.Element);
            
            if (string.IsNullOrEmpty(TxtPorta.Text))
                throw new EmptyException("Informe a porta!", TxtPorta.Element);
           
            if (string.IsNullOrEmpty(TxtEmail.Text))
                throw new EmptyException("Informe o email!", TxtEmail.Element);

            if (string.IsNullOrEmpty(TxtSenha.Text))
                throw new EmptyException("Informe a senha!", TxtSenha.Element);
            
            if (TxtSenha.Text != TxtConfirmarSenha.Text)
                throw new EmptyException("A confirmação da senha está diferente da senha informada!", TxtConfirmarSenha.Element);
            

            EditItemViewLayout.ViewModel.Smtp = TxtSmtp.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Porta = TxtPorta.Text.ToIntOrNull();
            EditItemViewLayout.ViewModel.Email = TxtEmail.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Senha = TxtSenha.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Ssl = ChkSSL.Checked;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
            }
            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Delete(object args)
        {
            foreach(var item in (List<UsuarioEmail>)args) ListItemViewLayout.ListItemView.Remove(item);
            EditItemViewLayout.ViewModal.Hide();
        }

        protected async Task BtnTestar_Click()
        {
            try
            {

                await HelpLoading.Show(this, "Testando configurações...");

                var Request = new Request();

                Request.Parameters.Add(new Parameters("Smtp", TxtSmtp.Text));
                Request.Parameters.Add(new Parameters("Porta", TxtPorta.Text));
                Request.Parameters.Add(new Parameters("Email", TxtEmail.Text));
                Request.Parameters.Add(new Parameters("Senha", TxtSenha.Text));
                Request.Parameters.Add(new Parameters("EnableSsl", ChkSSL.Checked));
                Request.Parameters.Add(new Parameters("Para", new List<string> { TxtEmail.Text }));
                Request.Parameters.Add(new Parameters("Assunto", "Configuração de email"));
                Request.Parameters.Add(new Parameters("Mensagem", "Se você recebeu esse e-mail, isso significa que está funcionando normalmente!"));

                await HelpHttp.Send(Http, "api/Email/Send", Request);

                await JSRuntime.InvokeVoidAsync("alert", "Email enviado com sucesso!");

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }
    }
}