using Aplicativo.View.Layout.Component.ListView.Controls;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Aplicativo.View.Layout.Component.ListView
{

    public class ListItemViewLayoutPage : ComponentBase
    {

        [Parameter] public string Title { get; set; }

        [Parameter] public RenderFragment ViewPages { get; set; }

        [Parameter] public RenderFragment BtnPesquisa { get; set; }
        [Parameter] public RenderFragment BtnNovo { get; set; }
        [Parameter] public RenderFragment BtnExcluir { get; set; }
        
        [Parameter] public RenderFragment GridView { get; set; }


        public ListViewBtnPesquisaComponent ListViewBtnPesquisa { get; set; }
        public GridViewItemComponent GridViewItem { get; set; }

        protected List<object> _ListItemView { get; set; } = new List<object>();

        public List<object> ListItemView
        {
            get => _ListItemView;
            set
            {
                _ListItemView = value;
                StateHasChanged();
            }
        }

        protected void Component_Resize(object args)
        {

        }

        public void Refresh()
        {
            StateHasChanged();
        }


    }
}