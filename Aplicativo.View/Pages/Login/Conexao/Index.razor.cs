using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Core.Component;

namespace Aplicativo.View.Pages.Login.Conexao
{
    public class ViewIndexPage : HelpComponent
    {

        public Options OptTipo { get; set; }
        
        public ViewDominio ViewDominio { get; set; }
        public ViewConexao ViewConexao { get; set; }

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();

            var Dominio = await HelpConexao.GetDominio(JSRuntime);

            if (!string.IsNullOrEmpty(Dominio.Name))
            {

                if (await HelpParametros.VerificarUsuarioLogado(JSRuntime))
                {
                    NavigationManager.NavigateTo("");
                    return;
                }

                NavigationManager.NavigateTo("Login/Entrar");
                return;
            }

            OptTipo.Value = "Dominio";

        }

        protected void OptTipo_Change()
        {

            StateHasChanged();

            if (OptTipo.Value == "Dominio")
            {
                ViewDominio.TxtName.Focus();
            }

            if (OptTipo.Value == "Conexao")
            {
                ViewConexao.TxtName.Focus();
            }

        }

        protected async Task BtnExcluir_Click(string Name)
        {
            await HelpConexao.Remove(JSRuntime, Name);
            StateHasChanged();
        }

        protected async Task BtnSelecionar_Click(string Name)
        {
            await HelpConexao.SetName(JSRuntime, Name);
            NavigationManager.NavigateTo("Login/Entrar");
        }


        protected async Task HandleSignInAsync()
        {

            StateHasChanged();

            //using (var db = new Context())
            //{

            //}

            //System.Console.WriteLine("HandleSignIn");

            //await DomHelpers.GoBackAsync();

            //System.Console.WriteLine("HandleSignInDone");
        }

    }
}
