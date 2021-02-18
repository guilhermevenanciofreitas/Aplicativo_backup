using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Model;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Material.Tab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout
{

    public enum ItemViewMode
    {
        New,
        Edit,
    }

    public class EditItemViewLayoutPage : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout ListItemViewLayout { get; set; }

        public ViewModal ViewModal { get; set; }

        public ItemViewMode ItemViewMode { get; set; }

        [Parameter] public string Title { get; set; }
        [Parameter] public string Width { get; set; }

        [Parameter] public EventCallback OnLimpar { get; set; }
        [Parameter] public EventCallback<object> OnCarregar { get; set; }
        [Parameter] public EventCallback OnSalvar { get; set; }
        [Parameter] public EventCallback OnExcluir { get; set; }

        [Parameter] public RenderFragment View { get; set; }

        protected async void BtnLimpar_Click()
        {
            try
            {
                ItemViewMode = ItemViewMode.New;
                await OnLimpar.InvokeAsync(null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        public async Task Carregar(long? ID)
        {

            if (ID == null)
            {
                ItemViewMode = ItemViewMode.New;
                await OnLimpar.InvokeAsync(null);
                return;
            }

            ItemViewMode = ItemViewMode.Edit;
            await OnCarregar.InvokeAsync(ID);
            ViewModal.Show();
            StateHasChanged();

        }

        protected async void BtnSalvar_Click()
        {
            try
            {

                await HelpLoading.Show(this, "Salvando...");
                await OnSalvar.InvokeAsync(null);
                ViewModal.Hide();
                await ListItemViewLayout.BtnPesquisar_Click();
                StateHasChanged();

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        protected async void BtnExcluir_Click()
        {
            try
            {

                var confirm = await JSRuntime.InvokeAsync<bool>("confirm", "Tem certeza que deseja excluir ?");
                if (!confirm) return;

                await HelpLoading.Show(this, "Excluindo...");
                await OnExcluir.InvokeAsync(null);
                ViewModal.Hide();
                await ListItemViewLayout.BtnPesquisar_Click();
                StateHasChanged();

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }


    }
}