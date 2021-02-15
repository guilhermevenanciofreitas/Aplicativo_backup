//using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public class HelpComponent : ComponentBase
    {

        //public HelpComponent()
        //{
        //    Http = new HttpClient() { BaseAddress = new Uri("http://192.168.0.6:7070/") };
        //}

        //[Inject] private ISessionStorageService Session { get; set; }

        [Inject] public HttpClient Http { get; set; }

        [Inject] public IJSRuntime JSRuntime { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }


        protected Parametros Parametros { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
            //    if (!await VerificarUsuarioLogado()) return;
            //    Parametros = await Session.GetItemAsync<Parametros>("Parametros");
            //}
        }

        public async Task<bool> VerificarUsuarioLogado()
        {
            return true;
            //return await HelpParametros.VerificarUsuarioLogado(Session, JSRuntime);
        }
    }
}