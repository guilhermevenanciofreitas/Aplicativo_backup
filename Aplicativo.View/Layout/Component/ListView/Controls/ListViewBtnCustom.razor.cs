using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{

    public partial class ListViewBtnCustomComponent<Type> : ComponentBase
    {

        [Parameter] public ListItemViewLayout<Type> ListItemViewLayout { get; set; }

        [Parameter] public string Text { get; set; } = "Novo";

        [Parameter] public string Color { get; set; } = "#5cb85c";

        [Parameter] public bool Visible { get; set; } = true;

        [Parameter] public bool Enabled { get; set; } = true;

        [Parameter] public string Width { get; set; } = "130px";

        [Parameter] public EventCallback OnClick { get; set; }

        protected async Task ButtonCustom_Click()
        {
            try
            {

                var List = ListItemViewLayout.ListViewGridView?.ListItemView?.Where(c => Convert.ToBoolean(c.GetType().GetProperty("Selected").GetValue(c)) == true)?.ToList();

                await OnClick.InvokeAsync(List);

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}
