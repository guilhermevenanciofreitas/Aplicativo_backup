using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public class HelpCookie
    {
        public static async Task Set(IJSRuntime JSRuntime, string Name, string Value, int Minutes)
        {
            await JSRuntime.InvokeVoidAsync("Cookie.set", Name, Value, Minutes);
        }
        public static async Task<string> Get(IJSRuntime JSRuntime, string Name)
        {
            return await JSRuntime.InvokeAsync<string>("Cookie.get", Name);
        }
        public static async Task Delete(IJSRuntime JSRuntime, string Name)
        {
            await JSRuntime.InvokeVoidAsync("Cookie.delete", Name);
        }
    }
}