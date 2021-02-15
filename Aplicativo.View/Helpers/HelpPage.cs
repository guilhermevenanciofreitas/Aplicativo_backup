using Microsoft.AspNetCore.Components;

namespace Aplicativo.View.Helpers
{
    public class HelpPage : HelpComponent
    {

        [Parameter] public EventCallback OnClose { get; set; }

        [Parameter] public EventCallback OnSave { get; set; }

        protected void Close()
        {
            OnClose.InvokeAsync(null);
        }

        protected void Save()
        {
            OnSave.InvokeAsync(null);
        }

    }
}
