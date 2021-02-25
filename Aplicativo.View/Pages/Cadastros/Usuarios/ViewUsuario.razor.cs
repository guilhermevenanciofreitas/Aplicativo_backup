﻿using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Material.Icon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public class ViewUsuarioPage : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout<Usuario> ListItemViewLayout { get; set; }
        public EditItemViewLayout<Usuario> EditItemViewLayout { get; set; }
        

        protected TextBox TxtNome { get; set; }
        protected TextBox TxtLogin { get; set; }
        protected TextBox TxtSenha { get; set; }

        protected TabSet TabSet { get; set; }

        protected ViewUsuarioEmail ViewUsuarioEmail { get; set; }

        private Usuario Usuario = new Usuario();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

                EditItemViewLayout.ItemViewButtons.Add(new ItemViewButton() { Icon = new FilterListIcon(), Label = "Imprimir", OnClick = Imprimir });
                EditItemViewLayout.ItemViewButtons.Add(new ItemViewButton() { Icon = new FilterListIcon(), Label = "Compartilhar", OnClick = Compartilhar });

            }
        }

        private async void Imprimir()
        {
            await JSRuntime.InvokeVoidAsync("alert", "Imprimir");
        }

        private async void Compartilhar()
        {
            await JSRuntime.InvokeVoidAsync("alert", "Compartilhar");
        }

        protected async Task ViewLayout_Limpar()
        {

            Usuario = new Usuario();

            TxtNome.Text = null;
            TxtLogin.Text = null;
            TxtSenha.Text = null;


            ViewUsuarioEmail.ListItemViewLayout.ListItemView = new List<UsuarioEmail>();
            ViewUsuarioEmail.ListItemViewLayout.Refresh();


            TxtNome.Focus();

            await TabSet.Active("Principal");

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Request = new Request();

            Request.Parameters.Add(new Parameters("UsuarioID", ((Usuario)args)?.UsuarioID));

            Usuario = await HelpHttp.Send<Usuario>(Http, "api/Usuario/Get", Request);

            TxtNome.Text = Usuario.Nome;
            TxtLogin.Text = Usuario.Login;
            TxtSenha.Text = Usuario.Senha;

            ViewUsuarioEmail.ListItemViewLayout.ListItemView = Usuario.UsuarioEmail.ToList();
            ViewUsuarioEmail.ListItemViewLayout.Refresh();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtNome.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe o nome!", TxtNome.Element);
            }

            if (string.IsNullOrEmpty(TxtLogin.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe o nome!", TxtLogin.Element);
            }

            Usuario.Nome = TxtNome.Text.ToStringOrNull();
            Usuario.Login = TxtLogin.Text.ToStringOrNull();
            Usuario.Senha = TxtSenha.Text.ToStringOrNull();

            Usuario.UsuarioEmail = ViewUsuarioEmail.ListItemViewLayout.ListItemView;

            var Request = new Request();

            var Usuarios = new List<Usuario> { Usuario };

            Request.Parameters.Add(new Parameters("Usuarios", Usuarios));

            Usuarios = await HelpHttp.Send<List<Usuario>>(Http, "api/Usuario/Save", Request);

            Usuario = Usuarios.FirstOrDefault();

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                await EditItemViewLayout.Carregar(Usuario);
            }
            else
            {
                EditItemViewLayout.ViewModal.Hide();
            }
            

        }

        protected async Task ViewLayout_Excluir()
        {

            var Request = new Request();

            var Usuarios = new List<int?> { Usuario.UsuarioID };

            Request.Parameters.Add(new Parameters("Usuarios", Usuarios));

            await HelpHttp.Send(Http, "api/Usuario/Delete", Request);

        }
    }
}