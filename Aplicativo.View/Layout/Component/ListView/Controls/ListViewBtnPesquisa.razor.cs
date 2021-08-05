using Aplicativo.Utils.Helpers;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{
    public class ListViewBtnPesquisaComponent : ComponentBase
    {

        [Parameter] public ViewFiltro ViewFiltro { get; set; }

        [Parameter] public string Text { get; set; } = "Pesquisar";

        [Parameter] public EventCallback OnClick { get; set; }

        private bool Excuting { get; set; }

        public async Task BtnPesquisar_Click()
        {
            try
            {

                if (Excuting) return;

                Excuting = true;

                await HelpLoading.Show("Carregando...");

                foreach (var item in ViewFiltro.Filtros)
                {
                    item.Search = new object[] { ((TextBox)item.Element[0]).Text };
                }

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
            finally
            {
                await HelpLoading.Hide();
                Excuting = false;
            }
        }

    }
}