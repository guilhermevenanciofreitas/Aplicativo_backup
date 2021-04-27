using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    [Serializable]
    public class HelpEmptyException : Exception
    {

        public static async Task New(IJSRuntime JSRunTime, string Message)
        {
            await JSRunTime.InvokeVoidAsync("alert", Message);
            throw new HelpEmptyException();
        }

        public static async Task New(IJSRuntime JSRunTime, ElementReference Element, string Message)
        {
            await JSRunTime.InvokeVoidAsync("alert", Message);
            Element.Focus(JSRunTime);
            throw new HelpEmptyException();
        }

        public HelpEmptyException() : base() { }
        public HelpEmptyException(string message) : base(message) { }
        
        public HelpEmptyException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected HelpEmptyException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
