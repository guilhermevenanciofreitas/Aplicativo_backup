using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout
{
    public partial class MenuLayout: ComponentBase
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await App.JSRuntime.InvokeVoidAsync("Menu.AfterRender");
        }
    }
}