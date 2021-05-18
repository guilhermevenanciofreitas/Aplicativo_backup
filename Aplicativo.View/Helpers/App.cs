using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Aplicativo.View.Helpers
{
    public class App
    {

        public static HttpClient Http { get; set; }

        public static IJSRuntime JSRuntime { get; set; }

        public static NavigationManager NavigationManager { get; set; }

    }
}