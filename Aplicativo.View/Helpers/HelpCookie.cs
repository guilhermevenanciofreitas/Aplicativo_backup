using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public class HelpCookie
    {
        public static async Task Set(string Name, string Value, int Minutes)
        {
            await App.JSRuntime.InvokeVoidAsync("Cookie.set", Name, Value, Minutes);
        }
        public static async Task<string> Get(string Name)
        {
            return await App.JSRuntime.InvokeAsync<string>("Cookie.get", Name);
        }
        public static async Task Delete(string Name)
        {
            await App.JSRuntime.InvokeVoidAsync("Cookie.delete", Name);
        }
    }
}