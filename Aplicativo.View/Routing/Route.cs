using System;

namespace Aplicativo.View.Routing
{
    public class Route
    {
        public string Uri { get; set; }
        public Type Component { get; set; }
        public bool LoginRequired { get; set; }
        public bool DatabaseRequired { get; set; }
    }
}