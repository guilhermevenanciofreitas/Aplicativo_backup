using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component
{
    public class ComponentLayoutBase : ComponentBase//, IDisposable
    {

        protected static Action<int, int> OnResizeFromJS;

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback OnLoad { get; set; }

        [Parameter] public EventCallback OnResize { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            OnResizeFromJS = Resize;
            await HelpLoading.Hide();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {

                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {

                    await App.JSRuntime.InvokeVoidAsync("WindowResize");
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

        protected void Resize(int Width, int Height)
        {

            HelpDisplay.Display = new Display(Width, Height);

            OnResize.InvokeAsync(HelpDisplay.Display);

            StateHasChanged();

        }

        [JSInvokable]
        public static Task PageResizeJSInvokable(int Width, int Height)
        {
            OnResizeFromJS.Invoke(Width, Height);
            return Task.FromResult(new List<int> { Width, Height });
        }

    }
}