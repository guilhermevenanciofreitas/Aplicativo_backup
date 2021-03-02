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

        protected TextBox TxtSmtp { get; set; }
        protected TextBox TxtPorta { get; set; }
        protected TextBox TxtEmail { get; set; }
        protected TextBox TxtSenha { get; set; }
        protected TextBox TxtConfirmarSenha { get; set; }

        protected CheckBox ChkSSL { get; set; }


        protected ProdutoFornecedor ProdutoFornecedor { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

            }
        }

        protected void ViewLayout_Limpar()
        {

            ProdutoFornecedor = new ProdutoFornecedor();

            //TxtSmtp.Text = null;
            //TxtPorta.Text = null;
            //TxtEmail.Text = null;
            //TxtSenha.Text = null;
            //TxtConfirmarSenha.Text = null;
            //ChkSSL.Checked = true;

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((ProdutoFornecedor)args);
            EditItemViewLayout.ViewModal.Show();
        }

        protected void ViewLayout_Carregar(object args)
        {

            ProdutoFornecedor = (ProdutoFornecedor)args;

            //TxtSmtp.Text = UsuarioEmail.Smtp.ToStringOrNull();
            //TxtPorta.Text = UsuarioEmail.Porta.ToStringOrNull();
            //TxtEmail.Text = UsuarioEmail.Email.ToStringOrNull();
            //TxtSenha.Text = UsuarioEmail.Senha.ToStringOrNull();
            //TxtConfirmarSenha.Text = UsuarioEmail.Senha.ToStringOrNull();
            //ChkSSL.Checked = UsuarioEmail.Ssl.ToBoolean();

        }

        protected void ViewLayout_Salvar()
        {

            //UsuarioEmail.Smtp = TxtSmtp.Text.ToStringOrNull();
            //UsuarioEmail.Porta = TxtPorta.Text.ToIntOrNull();
            //UsuarioEmail.Email = TxtEmail.Text.ToStringOrNull();
            //UsuarioEmail.Senha = TxtSenha.Text.ToStringOrNull();
            //UsuarioEmail.Ssl = ChkSSL.Checked;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(ProdutoFornecedor);
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