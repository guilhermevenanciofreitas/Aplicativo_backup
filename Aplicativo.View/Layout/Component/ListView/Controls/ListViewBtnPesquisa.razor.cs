using Aplicativo.Utils.Helpers;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
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

        public async Task BtnPesquisar_Click()
        {
            try
            {

                await HelpLoading.Show("Carregando...");

                foreach (var item in ViewFiltro.Filtros)
                {
                    item.Search = new object[] { ((TextBox)item.Element[0]).Text };
                }

                await OnClick.InvokeAsync(null);

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
            finally
            {
                await HelpLoading.Hide();
            }
        }

    }
}