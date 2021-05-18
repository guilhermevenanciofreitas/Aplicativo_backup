using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{
    public class ListViewBtnNovoComponent : ComponentBase
    {

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
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}
