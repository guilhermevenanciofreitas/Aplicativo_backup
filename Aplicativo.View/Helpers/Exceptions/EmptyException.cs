using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace Aplicativo.View.Helpers.Exceptions
{
    [Serializable]
    public class EmptyException : Exception
    {
        public EmptyException(string message) : base(message)
        {
            App.JSRuntime.InvokeVoidAsync("alert", Message);
        }

        public EmptyException(string message, ElementReference Element) : base(message) 
        {
            App.JSRuntime.InvokeVoidAsync("alert", Message);
            Element.Focus();
        }

    }
}