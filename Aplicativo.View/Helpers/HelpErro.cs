using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public static class HelpErro
    {

        public static async Task Show(HelpComponent HelpComponent, Exception Exception)
        {
            await HelpComponent.JSRuntime.InvokeVoidAsync("alert", Exception.Message);
        }

    }
}