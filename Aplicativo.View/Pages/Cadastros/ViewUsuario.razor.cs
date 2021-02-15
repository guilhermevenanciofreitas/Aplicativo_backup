using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Model;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.JSInterop;
using Skclusive.Material.Tab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros
{
    public class ViewUsuarioPage : HelpPage
    {

        public Usuario Usuario { get; set; } = new Usuario();


        protected ViewModal ViewModalUsuarioEmail { get; set; }

        protected Tabs TabUsuario { get; set; }
        protected object TabUsuarioItem { set; get; } = 0;

        protected TextBox TxtNome { get; set; }
        protected TextBox TxtLogin { get; set; }
        protected TextBox TxtSenha { get; set; }


        private void Limpar()
        {

            Usuario = new Usuario();

            TxtNome.Text = null;
            TxtLogin.Text = null;
            TxtSenha.Text = null;

        }

        public async Task Carregar(int? UsuarioID)
        {

            if (UsuarioID == null)
            {
                Limpar();
                return;
            }

            var Request = new Request();

            Request.Parameters.Add(new Parameters("UsuarioID", UsuarioID));

            Usuario = await HelpHttp.Send<Usuario>(Http, "api/Usuario/Get", Request);

            TxtNome.Text = Usuario.Nome;
            TxtLogin.Text = Usuario.Login;
            TxtSenha.Text = Usuario.Senha;

        }

        protected async void BtnSalvar_Click()
        {
            try
            {

                await HelpLoading.Show(this, "Salvando...");

                Usuario.Nome = TxtNome.Text.ToStringOrNull();
                Usuario.Login = TxtLogin.Text.ToStringOrNull();
                Usuario.Senha = TxtSenha.Text.ToStringOrNull();


                var Request = new Request();

                var Usuarios = new List<Usuario>
                {
                    Usuario
                };

                Request.Parameters.Add(new Parameters("Usuarios", Usuarios));

                Usuarios = await HelpHttp.Send<List<Usuario>>(Http, "api/Usuario/Save", Request);

                Usuario = Usuarios.FirstOrDefault();

                Save();

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

        protected async void BtnExcluir_Click()
        {
            try
            {

                await HelpLoading.Show(this, "Excluindo...");

                var Request = new Request();

                var Usuarios = new List<int?>
                {
                    Usuario.UsuarioID
                };

                Request.Parameters.Add(new Parameters("Usuarios", Usuarios));

                await HelpHttp.Send(Http, "api/Usuario/Delete", Request);

                Save();

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

        protected async void BtnAdicionarEmail_Click()
        {
            try
            {

                ViewModalUsuarioEmail.Show();

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        protected void TabUsuario_Change(object value)
        {
            TabUsuarioItem = value;
        }

    }
}