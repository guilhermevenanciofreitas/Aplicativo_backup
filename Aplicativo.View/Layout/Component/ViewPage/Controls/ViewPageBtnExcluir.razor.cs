using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ViewPage.Controls
{
    public class ViewPageBtnExcluirComponent<Type> : ComponentBase
    {

        [Parameter] public ListItemViewLayout<Type> ListItemViewLayout { get; set; }

        [Parameter] public string Text { get; set; } = "Excluir";

        [Parameter] public string Width { get; set; } = "110px";

        [Parameter] public bool Visible { get; set; } = true;

        [Parameter] public EventCallback OnClick { get; set; }

        protected async Task BtnExcluir_Click()
        {
            try
            {

                var confirm = await App.JSRuntime.InvokeAsync<bool>("confirm", "Tem certeza que deseja excluir ?");

                if (!confirm) return;

                await OnClick.InvokeAsync(null);

                if (ListItemViewLayout?.ListViewBtnPesquisa != null)
                {
                    await ListItemViewLayout.ListViewBtnPesquisa.BtnPesquisar_Click();
                }

                if (ListItemViewLayout?.ListViewGridView?.GridView != null)
                {
                    ListItemViewLayout.ListViewGridView.GridView.Refresh();
                }

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