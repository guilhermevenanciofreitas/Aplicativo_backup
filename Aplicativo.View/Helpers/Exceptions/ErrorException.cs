using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace Aplicativo.View.Helpers.Exceptions
{
    [Serializable]
    public class ErrorException : Exception
    {
        public ErrorException(string message) : base(message)
        {

        }
    }
}