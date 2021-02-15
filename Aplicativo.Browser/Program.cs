using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Aplicativo.Browser.Services;
using Aplicativo.View.Services;
using Skclusive.Material.Docs.App.View;
using Aplicativo.View.Helpers;

namespace Aplicativo.Browser
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<AppView>("#app");
            
            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<IBarCodeScanner, BarCodeScanner>();
            builder.Services.AddSingleton<ICamera, Camera>();

            builder.Services.TryAddViewServices
            (
                new HelpViewConfig()
                .WithIsServer(false)
                .WithIsPreRendering(true)
                .WithResponsive(true)
                .Build()
            );

            await builder.Build().RunAsync();

        }
    }
}