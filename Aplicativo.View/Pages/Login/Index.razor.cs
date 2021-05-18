using System.Threading.Tasks;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;

namespace Aplicativo.View.Pages.Login
{
    public class IndexPage : ComponentBase
    {

        public Options OptTipo { get; set; }
        
        public ViewDominio ViewDominio { get; set; }
        public ViewConexao ViewConexao { get; set; }

        protected void Component_Load()
        {
            OptTipo.Value = "Dominio";
        }

        protected async Task BtnExcluir_Click(string Name)
        {
            await HelpConexao.Remove(Name);
        }

        protected async Task BtnSelecionar_Click(string Name)
        {
            await HelpConexao.SetName(Name);
            App.NavigationManager.NavigateTo("Login/Entrar");
        }

    }
}