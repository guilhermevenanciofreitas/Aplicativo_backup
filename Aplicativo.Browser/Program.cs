using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Aplicativo.Browser.Services;
using Aplicativo.View.Services;
using Skclusive.Material.Docs.App.View;
using Aplicativo.View.Helpers;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

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

            builder.Services.AddSyncfusionBlazor();


            SyncfusionLicenseProvider.RegisterLicense("NDA3MzI5QDMxMzgyZTM0MmUzMFJYOFhwTm81eUlzL1g4QmJNNXZSRi9ZcDZSbS9Tcm9OY0ZIcjkrbmFMWG89");

            await builder.Build().RunAsync();

        }
    }
}