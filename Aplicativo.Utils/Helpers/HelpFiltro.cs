using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Aplicativo.Utils.Helpers
{

    public enum FiltroType {
        TextBox,
    }

    public class HelpFiltro
    {

        [JsonIgnore]
        public string Label { get; set; }

        public string Column { get; set; }

        [JsonIgnore]
        public object[] Element { get; set; }

        public FiltroType Type { get; set; }

        public object[] Search { get; set; }

    }
}
