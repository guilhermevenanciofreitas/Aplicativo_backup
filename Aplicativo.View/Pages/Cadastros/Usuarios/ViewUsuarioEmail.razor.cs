using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public class ViewUsuarioEmailPage : ComponentBase
    {

        public UsuarioEmail ViewModel { get; set; } = new UsuarioEmail();

        public ListItemViewLayout ListItemViewLayout { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        public TextBox TxtSmtp { get; set; }
        public TextBox TxtPorta { get; set; }
        public TextBox TxtEmail { get; set; }
        public TextBox TxtSenha { get; set; }
        public TextBox TxtConfirmarSenha { get; set; }

        public CheckBox ChkSSL { get; set; }


        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void ViewLayout_Limpar()
        {

            ViewModel = new UsuarioEmail();

            EditItemViewLayout.LimparCampos(this);

            ChkSSL.Checked = true;

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            ViewLayout_Limpar();

            await EditItemViewLayout.Show(args);

            if (args == null) return;

            ViewModel = (UsuarioEmail)args;

            TxtSmtp.Text = ViewModel.Smtp.ToStringOrNull();
            TxtPorta.Text = ViewModel.Porta.ToStringOrNull();
            TxtEmail.Text = ViewModel.Email.ToStringOrNull();
            TxtSenha.Text = ViewModel.Senha.ToStringOrNull();
            TxtConfirmarSenha.Text = ViewModel.Senha.ToStringOrNull();
            ChkSSL.Checked = ViewModel.Ssl.ToBoolean();

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

            var ListItemView = ListItemViewLayout.ListItemView;

            ViewModel.Smtp = TxtSmtp.Text.ToStringOrNull();
            ViewModel.Porta = TxtPorta.Text.ToIntOrNull();
            ViewModel.Email = TxtEmail.Text.ToStringOrNull();
            ViewModel.Senha = TxtSenha.Text.ToStringOrNull();
            ViewModel.Ssl = ChkSSL.Checked;

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

            foreach (var item in ((IEnumerable)args).Cast<UsuarioEmail>().ToList())
            {
                ListItemView.Remove(item);
            }

            ListItemViewLayout.ListItemView = ListItemView;

            EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task BtnTestar_Click()
        {
            try
            {

                await HelpLoading.Show("Testando configurações...");

                var Request = new Request();

                Request.Parameters.Add(new Parameters("Smtp", TxtSmtp.Text));
                Request.Parameters.Add(new Parameters("Porta", TxtPorta.Text));
                Request.Parameters.Add(new Parameters("Email", TxtEmail.Text));
                Request.Parameters.Add(new Parameters("Senha", TxtSenha.Text));
                Request.Parameters.Add(new Parameters("EnableSsl", ChkSSL.Checked));
                Request.Parameters.Add(new Parameters("Para", new List<string> { TxtEmail.Text }));
                Request.Parameters.Add(new Parameters("Assunto", "Configuração de email"));
                Request.Parameters.Add(new Parameters("Mensagem", "Se você recebeu esse e-mail, isso significa que está funcionando normalmente!"));

                //await HelpHttp.Send(Http, "api/Email/Send", Request);

                await App.JSRuntime.InvokeVoidAsync("alert", "Email enviado com sucesso!");

            }
            catch (Exception ex)
            {
                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            finally
            {
                await HelpLoading.Hide();
            }
        }
        #endregion

    }
}