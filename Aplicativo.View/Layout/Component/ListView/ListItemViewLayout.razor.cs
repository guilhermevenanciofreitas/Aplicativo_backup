using Aplicativo.View.Layout.Component.ListView.Controls;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Aplicativo.View.Layout.Component.ListView
{

    public class ListItemViewLayoutPage<Type> : ComponentBase
    {

        [Parameter] public string Title { get; set; }

        [Parameter] public RenderFragment ViewPages { get; set; }

        [Parameter] public RenderFragment BtnNovo { get; set; }
        [Parameter] public RenderFragment BtnOneSelected { get; set; }
        [Parameter] public RenderFragment BtnOneMoreSelected { get; set; }
        [Parameter] public RenderFragment BtnExcluir { get; set; }
        [Parameter] public RenderFragment BtnFiltro { get; set; }
        [Parameter] public RenderFragment BtnPesquisa { get; set; }
        [Parameter] public RenderFragment GridView { get; set; }

        public ViewFiltro ViewFiltro { get; set; }
        public ListViewBtnNovo<Type> ListViewBtnNovo { get; set; }
        public ListViewBtnExcluir<Type> ListViewBtnExcluir { get; set; }
        public ListViewBtnPesquisa ListViewBtnPesquisa { get; set; }
        public ListViewGridView<Type> ListViewGridView { get; set; }

        [Parameter] public ListViewGridView<Type> ListViewGridView1 { get; set; }

        public List<Type> Items
        {
            get => ListViewGridView.ListItemView;
            set => ListViewGridView.ListItemView = value;
        }

        protected void Page_Resize(object args)
        {
            StateHasChanged();
        }

        public void Refresh()
        {
            StateHasChanged();
        }

    }
}