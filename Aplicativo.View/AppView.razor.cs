using Aplicativo.View.Helpers;
using Microsoft.JSInterop;
using Skclusive.Material.Theme;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aplicativo.View
{
    public class AppViewPage : HelpComponent
    {

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();

            HelpConexao.Dominio = await HelpConexao.GetDominio(JSRuntime);

            if (string.IsNullOrEmpty(HelpConexao.Dominio.Name))
            {
                NavigationManager.NavigateTo("Login/Conexao");
            }
            else
            {
                if (!await HelpParametros.VerificarUsuarioLogado(JSRuntime))
                {
                    NavigationManager.NavigateTo("Login/Entrar");
                }
            }

        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    base.OnAfterRender(firstRender);

        //    if (firstRender)
        //    {

        //        HelpConexao.Dominio = await HelpConexao.GetDominio(JSRuntime);

        //        if (string.IsNullOrEmpty(HelpConexao.Dominio.Name))
        //        {
        //            NavigationManager.NavigateTo("Login/Conexao");
        //        }
        //        else
        //        {
        //            NavigationManager.NavigateTo("Login/Entrar");
        //        }


        //    }

        //}

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

        public static ThemeValue Dark = ThemeFactory.CreateTheme(new ThemeConfig
        {
            Palette = new PaletteConfig
            {
                Type = PaletteType.Dark,

                Primary = new PaletteColorConfig
                {
                    Main = PaletteColors.Blue.X200
                },

                Secondary = new PaletteColorConfig
                {
                    Main = PaletteColors.Pink.X200
                },

                Background = new PaletteBackground
                {
                    Default = "#121212",

                    Custom = new Dictionary<string, string>
                {
                    { "level1", PaletteColors.Grey.X900 },

                    { "level2", "#333" },

                    { "appbar-color", "#fff" },

                    { "appbar-background-color", "#333" },
                },
                }
            }
        });

    }
}
