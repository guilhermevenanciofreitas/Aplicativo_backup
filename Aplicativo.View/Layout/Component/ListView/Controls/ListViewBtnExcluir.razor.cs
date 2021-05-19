using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{
    public class ListViewBtnExcluirComponent<Type> : ComponentBase
    {

        [Parameter] public ListItemViewLayout<Type> ListItemViewLayout { get; set; }

        [Parameter] public string Text { get; set; } = "Excluir";

        [Parameter] public bool Visible { get; set; } = true;

        [Parameter] public bool Enabled { get; set; } = true;

        [Parameter] public string Width { get; set; } = "130px";

        [Parameter] public EventCallback OnClick { get; set; }

        protected async Task ButtonExcluir_Click()
        {
            try
            {

                var confirm = await App.JSRuntime.InvokeAsync<bool>("confirm", "Tem certeza que deseja excluir ?");

                if (!confirm) return;

                await HelpLoading.Show("Excluindo...");

                var List = ListItemViewLayout.ListViewGridView?.ListItemView?.Where(c => Convert.ToBoolean(c.GetType().GetProperty("Selected").GetValue(c)) == true)?.ToList();

                await OnClick.InvokeAsync(List);

                if (ListItemViewLayout?.ListViewBtnPesquisa != null)
                {
                    await ListItemViewLayout.ListViewBtnPesquisa.BtnPesquisar_Click();
                }

                if (ListItemViewLayout?.ListViewGridView?.GridView != null)
                {
                    ListItemViewLayout.ListViewGridView.GridView.Refresh();
                }



                //await ShowToast("Informação:", List.Count() + " registro(s) excluído(s) com sucesso!", "e-toast-success", "e-success toast-icons");

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
