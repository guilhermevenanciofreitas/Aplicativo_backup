using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{
    public class ViewModalControl : ComponentBase
    {

        protected bool Open { get; set; } = false;

        protected ElementReference DivModal { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string Top { get; set; } = "0px";

        [Parameter] public string Width { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public bool Overlay { get; set; } = true;

        [Parameter] public EventCallback OnLoad { get; set; }

        [Parameter] public EventCallback OnHide { get; set; }

        public async Task Show()
        {
            try
            {
                Open = true;
                await App.JSRuntime.InvokeVoidAsync("Modal.Show", DivModal);
                await OnLoad.InvokeAsync(null);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        public async Task ShowAsync()
        {
            try
            {

                await Show();

                using (FinishConfirm = new CancellationTokenSource())
                {
                    await Task.Delay(-1, FinishConfirm.Token);
                }

            }
            catch (Exception)
            {
                //await HelpErro.Show(new Error(ex));
            }
        }

        public async Task Hide()
        {
            try
            {

                Open = false;
                await App.JSRuntime.InvokeVoidAsync("Modal.Hide", DivModal);
                await OnHide.InvokeAsync(null);

                FinishConfirmToken();

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        private void FinishConfirmToken()
        {
            if (FinishConfirm?.Token.CanBeCanceled ?? false)
            {
                FinishConfirm.Cancel();
            }
        }

        private CancellationTokenSource FinishConfirm;

    }

}