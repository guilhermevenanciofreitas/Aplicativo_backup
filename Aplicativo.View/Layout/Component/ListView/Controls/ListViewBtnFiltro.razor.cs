using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{

    public partial class ListViewBtnFiltroComponent : ComponentBase
    {

        [Parameter] public string Text { get; set; } = "Filtro";

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public ViewFiltro ViewFiltro { get; set; }

        public async Task ButtonFiltro_Click()
        {
            try
            {
                await ViewFiltro.ViewModal.Show();
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