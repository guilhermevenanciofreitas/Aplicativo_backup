using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Model;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Pages.Cadastros;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Core.Component;
using Skclusive.Material.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout
{


    public class ItemViewButton
    {

        public string Label { get; set; }

        public Action OnClick { get; set; }

    }

    public class ListItemViewLayoutPage : HelpComponent
    {

        public ViewModal ViewFiltro;
        public ViewModal ViewModal;

        public List<HelpFiltro> Filtros { get; set; } = new List<HelpFiltro>();

        [Parameter] public RenderFragment View { get; set; }
        [Parameter] public RenderFragment<ItemView> ItemView { get; set; }

        public List<ItemViewButton> ItemViewButtons { get; set; } = new List<ItemViewButton>();

        public List<ItemView> ListItemView { get; set; } = new List<ItemView>();


        [Parameter] public EventCallback<object> OnDelete { get; set; }
        [Parameter] public EventCallback<object> OnItemView { get; set; }
        [Parameter] public EventCallback OnPesquisar { get; set; }

        protected async void BtnFiltro_Click()
        {
            try
            {
                ViewFiltro.Show();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        protected async Task BtnPesquisar_Click()
        {
            try
            {
                await HelpLoading.Show(this, "Carregando...");
                await Pesquisar();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        protected async Task ViewFiltro_Confirmar()
        {
            try
            {
                ViewFiltro.Hide();
                await HelpLoading.Show(this, "Carregando...");
                await Pesquisar();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        private async Task Pesquisar()
        {

            foreach (var item in Filtros)
            {
                item.Search = new object[] { ((TextBox)item.Element[0]).Text };
            }

            await OnPesquisar.InvokeAsync(null);

        }

        protected async void BtnNovo_Click()
        {
            try
            {
                await OnItemView.InvokeAsync(null);
                ViewModal.Show();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        public async Task BtnExcluir_Click()
        {
            try
            {

                var confirm = await JSRuntime.InvokeAsync<bool>("confirm", "Tem certeza que deseja excluir ?");

                if (!confirm) return;

                await HelpLoading.Show(this, "Excluindo...");

                await OnDelete.InvokeAsync(ListItemView.Where(c => c.Bool01 == true).Select(c => c.Long01).ToList());

                await Pesquisar();

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

        public async void ViewModal_Save()
        {
            try
            {
                ViewModal.Hide();
                await Pesquisar();
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

        public async void ViewModal_Close()
        {
            try
            {
                ViewModal.Hide();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        protected async void ListItemView_Press(ItemView ItemView)
        {
            if (!ListItemView.Any(c => c.Bool01 == true))
            {
                try
                {
                    await HelpLoading.Show(this, "Carregando...");

                    await OnItemView.InvokeAsync(ItemView.Long01);
                    ViewModal.Show();

                    StateHasChanged();

                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
                }
                finally
                {
                    await HelpLoading.Hide(this);
                }
            }
            else
            {
                try
                {
                    ItemView.Bool01 = !ItemView.Bool01;
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
                }
            }
        }

        protected async Task ListItemView_LongPress(ItemView ItemView)
        {
            try
            {
                ItemView.Bool01 = !ItemView.Bool01;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        protected async Task ListItemView_Unmake()
        {
            try
            {
                foreach (var item in ListItemView)
                {
                    item.Bool01 = false;
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        protected async Task ViewFiltro_Close()
        {
            try
            {
                ViewFiltro.Hide();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        protected bool ItemViewButtonOpen { set; get; }

        protected IReference ItemViewOpenButtonRef { set; get; } = new Reference();

        protected void ItemViewOpen_Close(EventArgs args)
        {
            ItemViewButtonOpen = false;
            StateHasChanged();
        }

        protected void ItemViewOpen_Close(MenuCloseReason reason)
        {
            ItemViewButtonOpen = false;
            StateHasChanged();
        }

        protected void ItemViewButtonOpen_Show()
        {
            ItemViewButtonOpen = true;
            StateHasChanged();
        }


    }
}