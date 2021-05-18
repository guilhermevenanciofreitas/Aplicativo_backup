using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Material.Theme;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aplicativo.View
{
    public class AppViewPage : ComponentBase
    {

        protected string Body { get; set; }

        //[Inject] HttpClient Http { get; set; }

        //[Inject] IJSRuntime JSRuntime { get; set; }

        //[Inject] NavigationManager NavigationManager { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {

                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {

                    //App.NavigationManager = NavigationManager;
                    //App.JSRuntime = JSRuntime;
                    //App.Http = Http;

                }
            }
            catch (Exception ex)
            {
                Body = HelpErro.GetMessage(new Error(ex));
            }
        }

        public static ThemeValue Light = ThemeFactory.CreateTheme(new ThemeConfig
        {
            Palette = new PaletteConfig
            {

                Type = PaletteType.Light,

                Primary = new PaletteColorConfig
                {
                    Main = PaletteColors.Blue.X700
                },

                Secondary = new PaletteColorConfig
                {
                    Main = PaletteColors.Pink.A400.Darken(0.1m)
                },

                Background = new PaletteBackground
                {
                    Default = "#fff",

                    Custom = new Dictionary<string, string>
                {
                    { "level1", "#fff" },

                    { "level2", PaletteColors.Grey.X100 },

                    { "appbar-color", "var(--theme-palette-primary-contrast-text)" },

                    { "appbar-background-color", "var(--theme-palette-primary-main)" },
                },
                }
            }

        });

    }
}