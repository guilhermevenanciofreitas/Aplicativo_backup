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

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public class ViewUsuarioEmailPage : HelpComponent
    {

        public ListItemViewLayout<UsuarioEmail> ListItemViewLayout { get; set; }
        public EditItemViewLayout<UsuarioEmail> EditItemViewLayout { get; set; }

        protected TextBox TxtSmtp { get; set; }
        protected TextBox TxtPorta { get; set; }
        protected TextBox TxtEmail { get; set; }
        protected TextBox TxtSenha { get; set; }
        protected TextBox TxtConfirmarSenha { get; set; }

        protected CheckBox ChkSSL { get; set; }


        protected UsuarioEmail UsuarioEmail { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

            }
        }

        protected void ViewLayout_Limpar()
        {

            UsuarioEmail = new UsuarioEmail();

            TxtSmtp.Text = null;
            TxtPorta.Text = null;
            TxtEmail.Text = null;
            TxtSenha.Text = null;
            TxtConfirmarSenha.Text = null;
            ChkSSL.Checked = true;

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((UsuarioEmail)args);
            EditItemViewLayout.ViewModal.Show();
        }

        protected void ViewLayout_Carregar(object args)
        {

            UsuarioEmail = (UsuarioEmail)args;

            TxtSmtp.Text = UsuarioEmail.Smtp.ToStringOrNull();
            TxtPorta.Text = UsuarioEmail.Porta.ToStringOrNull();
            TxtEmail.Text = UsuarioEmail.Email.ToStringOrNull();
            TxtSenha.Text = UsuarioEmail.Senha.ToStringOrNull();
            TxtConfirmarSenha.Text = UsuarioEmail.Senha.ToStringOrNull();
            ChkSSL.Checked = UsuarioEmail.Ssl.ToBoolean();

        }

        protected void ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtSmtp.Text))
            {
                throw new EmptyException("Informe o SMTP!", TxtSmtp.Element);
            }

            if (string.IsNullOrEmpty(TxtPorta.Text))
            {
                throw new EmptyException("Informe a porta!", TxtPorta.Element);
            }

            if (string.IsNullOrEmpty(TxtEmail.Text))
            {
                throw new EmptyException("Informe o email!", TxtEmail.Element);
            }

            if (string.IsNullOrEmpty(TxtSenha.Text))
            {
                throw new EmptyException("Informe a senha!", TxtSenha.Element);
            }

            if (TxtSenha.Text != TxtConfirmarSenha.Text)
            {
                throw new EmptyException("A confirmação da senha está diferente da senha informada!", TxtConfirmarSenha.Element);
            }

            UsuarioEmail.Smtp = TxtSmtp.Text.ToStringOrNull();
            UsuarioEmail.Porta = TxtPorta.Text.ToIntOrNull();
            UsuarioEmail.Email = TxtEmail.Text.ToStringOrNull();
            UsuarioEmail.Senha = TxtSenha.Text.ToStringOrNull();
            UsuarioEmail.Ssl = ChkSSL.Checked;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(UsuarioEmail);
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