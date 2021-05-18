using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ViewPage.Controls
{
    public partial class ViewPageBtnLimparComponent : ComponentBase
    {

        [Parameter] public string Text { get; set; } = "Limpar";

        [Parameter] public string Width { get; set; } = "110px";

        [Parameter] public bool Visible { get; set; } = true;

        [Parameter] public EventCallback OnClick { get; set; }

        protected async Task BtnLimpar_Click()
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
