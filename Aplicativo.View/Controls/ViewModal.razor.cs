using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;

namespace Aplicativo.View.Controls
{
    public class ViewModalControl : HelpComponent
    {

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool FullScreen { get; set; } = true;

        protected bool Open { get; set; } = false;

        public void Show()
        {
            Open = true;
        }

        public void Hide()
        {
            Open = false;
        }

    }
}