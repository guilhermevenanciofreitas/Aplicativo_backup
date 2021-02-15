using Microsoft.AspNetCore.Components;
using Skclusive.Material.Core;

namespace Aplicativo.View.Controls
{
    public class TabPanelControl : MaterialComponent
    {
        [Parameter]
        public object Value { set; get; }

        [Parameter]
        public object Index { set; get; }

        [Parameter]
        public string Label { set; get; }

        protected bool Hidden => !object.Equals(Value, Index);

        protected string _Id => $"{Id}-{Index}";

        protected string _Label => $"{Label}-{Index}";

    }
}
