using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicativo.Utils.Helpers
{
    [Serializable]
    public class EmptyException : Exception
    {

        public ElementReference Element { get; set; }

        public EmptyException() : base() { }
        public EmptyException(string message) : base(message) { }
        public EmptyException(string message, ElementReference Element) : base(message) { this.Element = Element; }
        public EmptyException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected EmptyException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
