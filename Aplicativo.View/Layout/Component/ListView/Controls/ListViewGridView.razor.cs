using Aplicativo.Utils.Helpers;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{
    public partial class ListViewGridViewComponent<Type> : ComponentBase
    {

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback OnDataChange { get; set; }

        public List<Type> _ListItemView { get; set; } = new List<Type>();

        public List<Type> ListItemView
        {
            get => _ListItemView;
            set
            {

                _ListItemView.Clear();
                
                foreach (var item in value)
                {
                    _ListItemView.Add(item);
                }

                OnDataChange.InvokeAsync(null);

                if (GridView != null)
                {
                    GridView.Refresh();
                }

                StateHasChanged();

            }
        }



        #region ListView

        [Parameter] public RenderFragment<Type> ItemView { get; set; }

        protected async void ListItemView_Press(object ItemView)
        {
            if (!ListItemView.Any(c => Convert.ToBoolean(c.GetType().GetProperty("Selected").GetValue(c)) == true))
            {
                try
                {
                    await HelpLoading.Show("Carregando...");
                    await OnItemView.InvokeAsync(ItemView);
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
            else
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
        }

        protected async Task ListItemView_LongPress(object ItemView)
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

        public async Task ListItemView_Unmake()
        {
            try
            {
                foreach (var item in ListItemView)
                {
                    item.GetType().GetProperty("Selected").SetValue(item, false);
                }

                ListItemView = ListItemView;

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }
        #endregion

        #region GridView

        [Parameter] public string CheckBoxWidth { get; set; } = "60";

        [Parameter] public bool CheckBoxVisible { get; set; } = true;

        [Parameter] public string Height { get; set; } = "60vh";

        [Parameter] public RenderFragment Columns { get; set; }

        public SfGrid<Type> GridView { get; set; }

        [Parameter] public EventCallback OnItemView { get; set; }

        [Parameter] public bool AllowGrouping { get; set; } = false;

        [Parameter] public bool AllowPaging { get; set; } = true;

        [Parameter] public bool ShowRegistroSelecionados { get; set; } = true;

        protected async void GridViewItem_DoubleClick(RecordDoubleClickEventArgs<Type> ItemView)
        {
            try
            {

                await HelpLoading.Show("Carregando...");

                await OnItemView.InvokeAsync(ItemView.RowData);

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
                await OnDataChange.InvokeAsync(null);
                //ListItemView = ListItemView;
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
                await OnDataChange.InvokeAsync(null);
                //ListItemView = ListItemView;
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        //public void Refresh()
        //{

        //    var ListView = ListItemView.Clone();

        //    ListItemView.Clear();
        //    ListItemView = new ObservableCollection<Type>();

        //    ListItemView = ListView;

        //    GridView.Refresh();

        //    GridView.RefreshColumns();

        //    StateHasChanged();

        //}

        #endregion

    }
}