using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace Aplicativo.View.Helpers
{
    public static class HelpElementReference
    {
        public static async void Focus(this ElementReference ElementReference)
        {
            try
            {
                await App.JSRuntime.InvokeVoidAsync("ElementReference.Focus", ElementReference);
            }
            catch (Exception ex)
            {
                await App.JSRuntime.InvokeVoidAsync("console.warn", ex.Message);
            }
        }
    }
}