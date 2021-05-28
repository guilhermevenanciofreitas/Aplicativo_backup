using Microsoft.AspNetCore.Components;

namespace Aplicativo.View.Pages.Login
{
    public class LoginLayoutPage : ComponentBase
    {

        [Parameter] public RenderFragment ChildContent { get; set; }

        protected void Page_Resize(object args)
        {
            StateHasChanged();
        }

    }
}