using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{
    public partial class ListViewGridViewComponent<Type> : ComponentBase
    {

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback OnDoubleClick { get; set; }

        [Parameter] public EventCallback OnDataChange { get; set; }

        [Parameter] public bool AllowGrouping { get; set; }


        protected List<Type> _ListItemView { get; set; } = new List<Type>();

        public List<Type> ListItemView
        {
            get => _ListItemView;
            set
            {
                _ListItemView = value;
                OnDataChange.InvokeAsync(null);
                GridView.Refresh();
            }
        }

        public SfGrid<Type> GridView { get; set; }



        protected async void GridViewItem_DoubleClick(RecordDoubleClickEventArgs<Type> ItemView)
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

        protected async Task GridViewItem_Checked(Type ItemView)
        {
            try
            {
                ItemView.GetType().GetProperty("Selected").SetValue(ItemView, !Convert.ToBoolean(ItemView.GetType().GetProperty("Selected").GetValue(ItemView)));
                ListItemView = ListItemView;
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
                foreach (var item in ListItemView)
                {
                    item.GetType().GetProperty("Selected").SetValue(item, (bool)args.Value);
                }

                ListItemView = ListItemView;

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}