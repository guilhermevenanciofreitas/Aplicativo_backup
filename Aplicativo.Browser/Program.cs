using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Aplicativo.Browser.Services;
using Aplicativo.View.Services;
using Aplicativo.View.Helpers;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using Aplicativo.View;
using Aplicativo.View.Tasks;
using Aplicativo.View.Routing;
using BlazorPro.BlazorSize;
using ExpressionPowerTools.Core.Dependencies;
using ExpressionPowerTools.Serialization;
using ExpressionPowerTools.Serialization.EFCore.Http.Extensions;
using Tewr.Blazor.FileReader;

namespace Aplicativo.Browser
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<AppView>("#app");
            
            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddExpressionPowerToolsEFCore(new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddSingleton<IBarCodeScanner, BarCodeScanner>();
            builder.Services.AddSingleton<ICamera, Camera>();

            builder.Services.AddFileReaderService(options => options.UseWasmSharedBuffer = true);


            builder.Services.AddSingleton<RouteManager>();

            builder.Services.TryAddViewServices
            (
                new HelpViewConfig()
                .WithIsServer(false)
                .WithIsPreRendering(true)
                .WithResponsive(true)
                .Build()
            );

            builder.Services.AddSyncfusionBlazor();

            builder.Services.AddMediaQueryService();
            builder.Services.AddScoped<ResizeListener>();

            SyncfusionLicenseProvider.RegisterLicense("NDUyMjcxQDMxMzkyZTMxMmUzMGhaZWdaemtERXduQUF2ME1kallnMDZublVKMTBCTGYvb1BYZmNjczdoRG89");

            await builder.Build().RunAsync();

        }
    }
}