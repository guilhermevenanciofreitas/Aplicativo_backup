using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;

namespace Aplicativo.View.Pages.Login.Entrar
{
    public class LayoutPage : HelpComponent
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
    }
}
