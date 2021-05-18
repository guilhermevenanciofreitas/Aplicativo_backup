using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{
    public class GridViewItemComponent : ComponentBase
    {

        [CascadingParameter] public ListItemViewLayout ListItemViewLayout { get; set; }

        [Parameter] public EventCallback OnDoubleClick { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool AllowGrouping { get; set; }


        public SfGrid<object> GridViewItem { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            await base.OnAfterRenderAsync(firstRender);

            ListItemViewLayout.GridViewItem = this;

        }

        protected async void GridViewItem_DoubleClick(RecordDoubleClickEventArgs<object> ItemView)
        {
            try
            {

                await HelpLoading.Show("Carregando...");

                await OnDoubleClick.InvokeAsync(ItemView.RowData);

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

        protected async Task GridViewItem_Chcked(object ItemView)
        {
            try
            {
                ItemView.GetType().GetProperty("Selected").SetValue(ItemView, !Convert.ToBoolean(ItemView.GetType().GetProperty("Selected").GetValue(ItemView)));
                ListItemViewLayout.ListItemView = ListItemViewLayout.ListItemView;
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task GridViewItemHeader_Change(ChangeEventArgs args)
        {
            try
            {
                foreach (var item in ListItemViewLayout.ListItemView)
                {
                    item.GetType().GetProperty("Selected").SetValue(item, (bool)args.Value);
                }

                ListItemViewLayout.ListItemView = ListItemViewLayout.ListItemView;

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}