using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;

namespace Aplicativo.View.Pages.Login
{
    public class LoginLayoutPage : ComponentBase
    {

        [Parameter] public RenderFragment ChildContent { get; set; }

    }
}