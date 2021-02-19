using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Model;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros
{
    public class ViewUsuarioPage : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout ListItemViewLayout { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }
        

        protected TabSet TabPrincipal { get; set; }

        protected TextBox TxtNome { get; set; }
        protected TextBox TxtLogin { get; set; }
        protected TextBox TxtSenha { get; set; }

        private Usuario Usuario = new Usuario();

        protected void ViewLayout_Limpar()
        {

            Usuario = new Usuario();

            TxtNome.Text = null;
            TxtLogin.Text = null;
            TxtSenha.Text = null;

        }

        protected async Task ViewLayout_Carregar(object ID)
        {

            var Request = new Request();

            Request.Parameters.Add(new Parameters("UsuarioID", ID.ToString().ToIntOrNull()));

            Usuario = await HelpHttp.Send<Usuario>(Http, "api/Usuario/Get", Request);

            TxtNome.Text = Usuario.Nome;
            TxtLogin.Text = Usuario.Login;
            TxtSenha.Text = Usuario.Senha;

        }

        protected async Task ViewLayout_Salvar()
        {

            Usuario.Nome = TxtNome.Text.ToStringOrNull();
            Usuario.Login = TxtLogin.Text.ToStringOrNull();
            Usuario.Senha = TxtSenha.Text.ToStringOrNull();

            var Request = new Request();

            var Usuarios = new List<Usuario> { Usuario };

            Request.Parameters.Add(new Parameters("Usuarios", Usuarios));

            Usuarios = await HelpHttp.Send<List<Usuario>>(Http, "api/Usuario/Save", Request);

            Usuario = Usuarios.FirstOrDefault();

            StateHasChanged();

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