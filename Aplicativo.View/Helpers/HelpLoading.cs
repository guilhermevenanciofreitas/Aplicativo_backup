﻿using Aplicativo.View.Helpers;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public class HelpLoading
    {

        public static async Task Show(HelpComponent helpComponent, string text = null) 
        {
            await Show(helpComponent.JSRuntime, text);
            //helpComponent.Refresh();
        }
        public static async Task Hide(HelpComponent helpComponent) 
        {
            await Hide(helpComponent.JSRuntime);
            //helpComponent.Refresh();
        }

        public static async Task Show(IJSRuntime JSRuntime, string text = null)
        {
            if (text == null) text = "Carregando...";
            await JSRuntime.InvokeVoidAsync("Loading", "block", text);
        }
        public static async Task Hide(IJSRuntime JSRuntime)
        {
            await JSRuntime.InvokeVoidAsync("Loading", "none", "");
        }

    }
}