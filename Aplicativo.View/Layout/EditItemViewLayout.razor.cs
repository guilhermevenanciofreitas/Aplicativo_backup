using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Model;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Core.Component;
using Skclusive.Material.Menu;
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

        public List<ItemViewButton> ItemViewButtons { get; set; } = new List<ItemViewButton>();

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
                await ListItemViewLayout.BtnPesquisar_Click();
                await ListItemViewLayout.ShowToast("Informação:", "Salvo com sucesso!", "e-toast-success", "e-success toast-icons");
                StateHasChanged();

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                ViewModal.Show();
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

                ViewModal.Hide();
                await HelpLoading.Show(this, "Excluindo...");
                await OnExcluir.InvokeAsync(null);
                await ListItemViewLayout.BtnPesquisar_Click();
                await ListItemViewLayout.ShowToast("Informação:", "Excluído com sucesso!", "e-toast-success", "e-success toast-icons");
                StateHasChanged();

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                ViewModal.Show();
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        protected bool ItemViewButtonOpen { set; get; }

        protected IReference ItemViewOpenButtonRef { set; get; } = new Reference();

        protected void ItemViewOpen_Close(EventArgs args)
        {
            ItemViewButtonOpen = false;
        }

        protected void ItemViewOpen_Close(MenuCloseReason reason)
        {
            ItemViewButtonOpen = false;
        }

        protected void ItemViewButtonOpen_Show()
        {
            ItemViewButtonOpen = true;
        }

    }
}