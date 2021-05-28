using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ViewPage.Controls
{
    public partial class ViewPageBtnSalvarComponent<Type> : ComponentBase
    {

        protected SfToast Toast { get; set; }

        [Parameter] public ListItemViewLayout<Type> ListItemViewLayout { get; set; }

        [Parameter] public string Text { get; set; } = "Salvar";

        [Parameter] public string Width { get; set; } = "110px";

        [Parameter] public EventCallback OnClick { get; set; }

        protected async Task BtnSalvar_Click()
        {
            try
            {

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