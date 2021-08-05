using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ViewPage.Controls
{
    public partial class ViewPageBtnCustomComponent : ComponentBase
    {

        [Parameter] public string Text { get; set; } = "";

        [Parameter] public string Color { get; set; } = "#5cb85c";

        [Parameter] public string Width { get; set; } = "110px";

        [Parameter] public bool Visible { get; set; } = true;

        [Parameter] public EventCallback OnClick { get; set; }

        protected async Task BtnCustom_Click()
        {
            try
            {

                await OnClick.InvokeAsync(null);

            }
            catch (ErrorException ex)
            {
                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            catch (EmptyException)
            {

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}
