using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aplicativo.View.Routing
{
    public class ConventionRouter : IComponent, IHandleAfterRender, IDisposable
    {

        RenderHandle _renderHandle;
        bool _navigationInterceptionEnabled;
        string _location;

        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private HttpClient Http { get; set; }
        [Inject] private INavigationInterception NavigationInterception { get; set; }
        [Inject] RouteManager RouteManager { get; set; }

        [Parameter] public RenderFragment NotFound { get; set; }
        [Parameter] public RenderFragment<RouteData> Found { get; set; }

        [Parameter] public string PageDefault { get; set; }

        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
            _location = NavigationManager.Uri;
            NavigationManager.LocationChanged += HandleLocationChanged;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (Found == null)
            {
                throw new InvalidOperationException($"The {nameof(ConventionRouter)} component requires a value for the parameter {nameof(Found)}.");
            }

            if (NotFound == null)
            {
                throw new InvalidOperationException($"The {nameof(ConventionRouter)} component requires a value for the parameter {nameof(NotFound)}.");
            }

            RouteManager.Initialise();
            Refresh();

            return Task.CompletedTask;
        }

        public Task OnAfterRenderAsync()
        {
            if (!_navigationInterceptionEnabled)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs args)
        {
            _location = args.Location;
            Refresh();
        }

        private async void Refresh()
        {

            App.NavigationManager = NavigationManager;
            App.JSRuntime = JSRuntime;
            App.Http = Http;

            HelpConexao.Dominio = await HelpConexao.GetDominio();


            var relativeUri = App.NavigationManager.ToBaseRelativePath(_location);
            var parameters = ParseQueryString(relativeUri);

            if (relativeUri.IndexOf('?') > -1)
            {
                relativeUri = relativeUri.Substring(0, relativeUri.IndexOf('?'));
            }


            Route Route = null;
            
            switch (relativeUri)
            {
                case "":

                    if (string.IsNullOrEmpty(HelpConexao.Dominio.Name))
                    {
                        Route = GetRoute("Login");
                        break;
                    }

                    if (!await HelpParametros.VerificarUsuarioLogado())
                    {
                        Route = GetRoute("Login/Entrar");
                        break;
                    }

                    Route = GetRoute(PageDefault);

                    break;

                case "Login":

                    if (!string.IsNullOrEmpty(HelpConexao.Dominio.Name))
                    {
                        Route = GetRoute("Login/Entrar");
                        break;
                    }

                    if (await HelpParametros.VerificarUsuarioLogado())
                    {
                        Route = GetRoute(PageDefault);
                        break;
                    }

                    Route = RouteManager.Routes.FirstOrDefault(c => c.Uri.ToLower() == relativeUri.ToLower());

                    break;

                case "Login/Entrar":

                    if (string.IsNullOrEmpty(HelpConexao.Dominio.Name))
                    {
                        Route = GetRoute("Login");
                        break;
                    }

                    if (await HelpParametros.VerificarUsuarioLogado())
                    {
                        Route = GetRoute(PageDefault);
                        break;
                    }

                    Route = GetRoute(relativeUri);

                    break;

                case "Login/Recuperar":

                    if (string.IsNullOrEmpty(HelpConexao.Dominio.Name))
                    {
                        Route = GetRoute("Login");
                        break;
                    }

                    if (await HelpParametros.VerificarUsuarioLogado())
                    {
                        Route = GetRoute(PageDefault);
                        break;
                    }

                    Route = GetRoute(relativeUri);

                    break;

                default:

                    Route = GetRoute(relativeUri);

                    break;

            }

            if (Route != null)
            {

                if (Route.LoginRequired)
                {
                    if (!await HelpParametros.VerificarUsuarioLogado())
                    {
                        Route = GetRoute("Login/Entrar");
                    }
                }

                if (Route.DatabaseRequired)
                {
                    if (string.IsNullOrEmpty(HelpConexao.Dominio.Name))
                    {
                        Route = GetRoute("Login");
                    }
                }

                await App.JSRuntime.InvokeVoidAsync("history.pushState", null, null, Route.Uri);

                _renderHandle.Render(Found(new RouteData(Route.Component, parameters)));

            }
            else
            {
                _renderHandle.Render(NotFound);
            }

        }

        private Route GetRoute(string relativeUri)
        {
            return RouteManager.Routes.FirstOrDefault(c => c.Uri.ToLower() == relativeUri.ToLower());
        }

        private Dictionary<string, object> ParseQueryString(string Uri)
        {
            var querystring = new Dictionary<string, object>();

            foreach (string kvp in Uri[(Uri.IndexOf("?") + 1)..].Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (kvp != "" && kvp.Contains("="))
                {
                    var pair = kvp.Split('=');
                    querystring.Add(pair[0], pair[1]);
                }
            }

            return querystring;
        }
    }
}