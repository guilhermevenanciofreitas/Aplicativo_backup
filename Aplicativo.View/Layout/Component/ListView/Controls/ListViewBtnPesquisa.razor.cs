using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{
    public class ListViewBtnPesquisaComponent : ComponentBase
    {

        [CascadingParameter] public ListItemViewLayout ListItemViewLayout { get; set; }

        [Parameter] public string Text { get; set; } = "Pesquisar";

        [Parameter] public EventCallback OnPesquisar { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            ListItemViewLayout.ListViewBtnPesquisa = this;

        }

        public async Task BtnPesquisar_Click()
        {
            try
            {

                await HelpLoading.Show("Carregando...");
                
                //foreach (var item in ViewFiltro.Filtros)
                //{
                //    item.Search = new object[] { ((TextBox)item.Element[0]).Text };
                //}

                await OnPesquisar.InvokeAsync(null);

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