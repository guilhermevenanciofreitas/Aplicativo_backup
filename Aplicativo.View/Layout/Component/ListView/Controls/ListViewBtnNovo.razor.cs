using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{
    public class ListViewBtnNovoComponent<Type> : ComponentBase
    {

        [Parameter] public ListItemViewLayout<Type> ListItemViewLayout { get; set; }

        [Parameter] public string Text { get; set; } = "Novo";

        [Parameter] public bool Visible { get; set; } = true;

        [Parameter] public bool Enabled { get; set; } = true;

        [Parameter] public string Width { get; set; } = "130px";

        [Parameter] public EventCallback OnClick { get; set; }

        protected async Task ButtonNovo_Click()
        {
            try
            {
                await OnClick.InvokeAsync(null);
            }
            catch (ErrorException ex)
            {
                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}
