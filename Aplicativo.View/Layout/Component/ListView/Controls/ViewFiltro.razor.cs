using Aplicativo.Utils.Helpers;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{

    public class ViewFiltroComponent : ComponentBase
    {

        public ViewModal ViewModal;

        [Parameter] public List<HelpFiltro> Filtros { get; set; } = new List<HelpFiltro>();


        [Parameter] public string Title { get; set; } = "Filtro";

        [Parameter] public EventCallback OnConfirmar { get; set; }

        protected async Task ViewFiltro_Confirmar()
        {
            try
            {

                await ViewModal.Hide();

                await OnConfirmar.InvokeAsync(null);

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
