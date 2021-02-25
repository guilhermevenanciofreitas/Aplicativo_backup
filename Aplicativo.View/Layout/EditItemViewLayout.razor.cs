using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
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

    public class EditItemViewLayoutPage<TValue> : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }

        public ViewModal ViewModal { get; set; }

        public ItemViewMode ItemViewMode { get; set; }

        [Parameter] public string Title { get; set; }
        [Parameter] public string Width { get; set; }
        [Parameter] public bool Simples { get; set; } = false;

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

        public async Task Carregar(object args)
        {

            await OnLimpar.InvokeAsync(null);

            if (args == null)
            {
                ItemViewMode = ItemViewMode.New;
            }
            else
            {
                ItemViewMode = ItemViewMode.Edit;
                await OnCarregar.InvokeAsync(args);
            }
            
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

                if (HelpParametros.Template == Template.Desktop)
                {
                    ListItemViewLayout.GridViewItem.Refresh();
                }

                StateHasChanged();

            }
            catch (EmptyException ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                ex.Element.Focus(JSRuntime);
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

                ViewModal.Hide();
                await HelpLoading.Show(this, "Excluindo...");
                await OnExcluir.InvokeAsync(null);
                await ListItemViewLayout.BtnPesquisar_Click();
                await ListItemViewLayout.ShowToast("Informação:", "Excluído com sucesso!", "e-toast-success", "e-success toast-icons");

                ListItemViewLayout.GridViewItem.Refresh();
                ListItemViewLayout.Refresh();

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