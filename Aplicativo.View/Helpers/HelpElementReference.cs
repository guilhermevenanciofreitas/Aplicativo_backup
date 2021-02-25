using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Aplicativo.View.Helpers
{
    public static class HelpElementReference
    {
        public static async void Focus(this ElementReference ElementReference, IJSRuntime JSRuntime)
        {
            await JSRuntime.InvokeVoidAsync("ElementReference.Focus", ElementReference);
        }
    }
}