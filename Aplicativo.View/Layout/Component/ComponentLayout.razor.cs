using Aplicativo.View.Helpers;
using BlazorPro.BlazorSize;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component
{
    public class ComponentLayoutBase : ComponentBase
    {

        [Inject] protected ResizeListener Listener { get; set; }

        protected BrowserWindowSize Browser = new BrowserWindowSize();

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback OnLoad { get; set; }

        [Parameter] public EventCallback OnResize { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await HelpLoading.Hide();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {

                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {

                    Listener.OnResized += Resize;

                    await OnLoad.InvokeAsync(null);

                    StateHasChanged();

                }
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
            finally
            {
                await HelpLoading.Hide();
            }
        }

        protected void Resize(object args, BrowserWindowSize window)
        {

            HelpDisplay.Display = new Display(window.Width, window.Height);

            OnResize.InvokeAsync(HelpDisplay.Display);

            Browser = window;
            StateHasChanged();

        }

    }
}