using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{
    public class ViewModalControl : HelpComponent
    {

        protected ElementReference DivModal { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool FullScreen { get; set; } = true;

        protected bool Open { get; set; } = false;

        public async void Show()
        {
            if (HelpParametros.Template == Template.Mobile)
            {
                Open = true;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("Modal.Show", DivModal);
            }

            StateHasChanged();

        }

        public async void Hide()
        {
            if (HelpParametros.Template == Template.Mobile)
            {
                Open = false;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("Modal.Hide", DivModal);
            }

            StateHasChanged();

        }

    }
}