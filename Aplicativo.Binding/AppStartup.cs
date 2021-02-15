using Aplicativo.Binding.Services;
using Aplicativo.View.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.MobileBlazorBindings;
using Skclusive.Material.Docs.App.View;
using Xamarin.Forms;

namespace Aplicativo.Binding
{
    public class AppStartup : Application
    {
        public AppStartup(string[] args = null, IFileProvider fileProvider = null)
        {

            var hostBuilder = MobileBlazorBindingsHost.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Adds web-specific services such as NavigationManager
                    services.AddBlazorHybrid();

                    services.AddSingleton<IBarCodeScanner, BarCodeScanner>();
                    services.AddSingleton<ICamera, Camera>();

                    services.TryAddViewServices
                    (
                         new HelpViewConfig()
                        .WithIsServer(false)
                        .WithIsPreRendering(false)
                        .WithResponsive(true)
                        .WithTheme(Skclusive.Core.Component.Theme.Auto)
                        // .WithDisableBinding(true)
                        .Build()
                    );
                })
                .UseWebRoot("wwwroot");

            if (fileProvider != null)
            {
                hostBuilder.UseStaticFiles(fileProvider);
            }
            else
            {
                hostBuilder.UseStaticFiles();
            }
            var host = hostBuilder.Build();

            var contentPage = new ContentPage { Title = "Skclusive Material" };
            // hiding the title bar
            NavigationPage.SetHasNavigationBar(contentPage, false);

            MainPage = contentPage;

            host.AddComponent<Main>(parent: MainPage);


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}