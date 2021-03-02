using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Core.Component;
using Skclusive.Material.Button;
using Skclusive.Material.Icon;
using Skclusive.Material.Menu;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout
{


    public class ItemViewButton
    {

        public Button Button { get; set; }

        public SvgIconBase Icon { get; set; }

        public string Label { get; set; }

        public bool Disabled { get; set; }

        public System.Action OnClick { get; set; }

    }

    public class ListItemViewLayoutPage<TValue> : HelpComponent
    {

        [Parameter] public string Height { get; set; } = "460px";

        public ViewModal ViewFiltro;

        public List<HelpFiltro> Filtros { get; set; } = new List<HelpFiltro>();

        [Parameter] public RenderFragment View { get; set; }
        [Parameter] public RenderFragment<TValue> ItemView { get; set; }

        [Parameter] public RenderFragment GridView { get; set; }

        public SfGrid<TValue> GridViewItem { get; set; }

        public List<ItemViewButton> ItemViewButtons { get; set; } = new List<ItemViewButton>();

        public List<TValue> ListItemView { get; set; } = new List<TValue>();


        [Parameter] public EventCallback<object> OnDelete { get; set; }
        [Parameter] public EventCallback<object> OnItemView { get; set; }
        [Parameter] public EventCallback OnPesquisar { get; set; }

        [Parameter] public bool Simples { get; set; } = false;

        protected async void BtnFiltro_Click()
        {
            try
            {
                ViewFiltro.Show();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        public async Task BtnPesquisar_Click()
        {
            try
            {
                await HelpLoading.Show(this, "Carregando...");
                await Pesquisar();
                GridViewItem.Refresh();
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

            StateHasChanged();

        }

        protected async void BtnNovo_Click()
        {
            try
            {
                await OnItemView.InvokeAsync(null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        protected async Task BtnExcluir_Click()
        {
            try
            {

                var confirm = await JSRuntime.InvokeAsync<bool>("confirm", "Tem certeza que deseja excluir ?");

                if (!confirm) return;

                await HelpLoading.Show(this, "Excluindo...");

                var List = ListItemView.Where(c => Convert.ToBoolean(c.GetType().GetProperty("Selected").GetValue(c)) == true).ToList();

                await OnDelete.InvokeAsync(List);

                await Pesquisar();

                await ShowToast("Informação:", List.Count() + " registro(s) excluído(s) com sucesso!", "e-toast-success", "e-success toast-icons");

                if (HelpParametros.Template == Template.Desktop)
                {
                    GridViewItem.Refresh();
                }
                
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

        protected async void ListItemView_Press(TValue ItemView)
        {
            if (!ListItemView.Any(c => Convert.ToBoolean(c.GetType().GetProperty("Selected").GetValue(c)) == true))
            {
                try
                {

                    await HelpLoading.Show(this, "Carregando...");
                    await OnItemView.InvokeAsync(ItemView);
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
                    //ItemView.Bool01 = !ItemView.Bool01;
                    ItemView.GetType().GetProperty("Selected").SetValue(ItemView, !Convert.ToBoolean(ItemView.GetType().GetProperty("Selected").GetValue(ItemView)));
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
                }
            }
        }

        protected async Task ListItemView_LongPress(TValue ItemView)
        {
            try
            {
                //ItemView.Bool01 = !ItemView.Bool01;

                ItemView.GetType().GetProperty("Selected").SetValue(ItemView, !Convert.ToBoolean(ItemView.GetType().GetProperty("Selected").GetValue(ItemView)));

                StateHasChanged();
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
                    //item.Bool01 = false;

                    //Convert.ToBoolean() == true

                    item.GetType().GetProperty("Selected").SetValue(item, false);


                }

                StateHasChanged();

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
                StateHasChanged();
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
        }

        protected void ItemViewOpen_Close(MenuCloseReason reason)
        {
            ItemViewButtonOpen = false;
        }

        protected void ItemViewButtonOpen_Show()
        {
            ItemViewButtonOpen = true;
        }

        protected async void GridViewItem_DoubleClick(RecordDoubleClickEventArgs<TValue> ItemView)
        {
            try
            {
                await HelpLoading.Show(this, "Carregando...");

                await OnItemView.InvokeAsync(ItemView.RowData);

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

        protected void GridViewItem_Chcked(TValue ItemView)
        {
            //ItemView.Bool01 = !ItemView.Bool01;
            ItemView.GetType().GetProperty("Selected").SetValue(ItemView, !Convert.ToBoolean(ItemView.GetType().GetProperty("Selected").GetValue(ItemView)));
            StateHasChanged();
        }

        protected void ListItemViewHeeader_Change(ChangeEventArgs args)
        {
            foreach (var item in ListItemView)
            {
                //item.Bool01 = (bool)args.Value;

                item.GetType().GetProperty("Selected").SetValue(item, (bool)args.Value);

            }

            StateHasChanged();

        }

        protected void MnuMarcarTodos_Click()
        {
            foreach (var item in ListItemView)
            {
                //item.Bool01 = true;

                item.GetType().GetProperty("Selected").SetValue(item, true);

            }

            ItemViewOpen_Close(null);

            StateHasChanged();
        }

        protected SfToast Toast;

        public async Task ShowToast(string Title, string Content, string CssClass = null, string Icon = null)
        {
            await Toast.Show(new ToastModel() { Title = Title, Content = Content, CssClass = CssClass, Icon = Icon });
        }
        public async Task HideToast()
        {
            await Toast.Hide("All");
        }

        public void Refresh()
        {
            //GridViewItem.Refresh();
            StateHasChanged();
        }

    }
}