using Aplicativo.View.Helpers;
using Aplicativo.View.Pages.Cadastros.Usuarios;
using Skclusive.Material.Core;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout
{
    public class AppLayoutComponent : MaterialLayoutComponent
    {

        protected ViewUsuario ViewUsuario { get; set; }

        protected async Task BtnSair_Click()
        {

            await HelpCookie.Delete("ManterConectado");

            HelpParametros.Parametros.UsuarioLogado = null;
            HelpParametros.Parametros.EmpresaLogada = null;

            App.NavigationManager.NavigateTo("Login/Entrar");
            
        }

        protected async void BtnSetting_Click()
        {

            await ViewUsuario.EditItemViewLayout.Show(HelpParametros.Parametros.UsuarioLogado);

        }

    }
}