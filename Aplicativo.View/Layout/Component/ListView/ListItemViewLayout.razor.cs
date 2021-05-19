using Aplicativo.View.Layout.Component.ListView.Controls;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Aplicativo.View.Layout.Component.ListView
{

    public class ListItemViewLayoutPage<Type> : ComponentBase
    {

        [Parameter] public string Title { get; set; }

        [Parameter] public RenderFragment ViewPages { get; set; }

        [Parameter] public RenderFragment BtnNovo { get; set; }
        [Parameter] public RenderFragment BtnExcluir { get; set; }
        [Parameter] public RenderFragment BtnFiltro { get; set; }
        [Parameter] public RenderFragment BtnPesquisa { get; set; }
        [Parameter] public RenderFragment GridView { get; set; }

        public ViewFiltro ViewFiltro { get; set; }
        public ListViewBtnNovo ListViewBtnNovo { get; set; }
        public ListViewBtnExcluir<Type> ListViewBtnExcluir { get; set; }
        public ListViewBtnPesquisa ListViewBtnPesquisa { get; set; }
        public ListViewGridView<Type> ListViewGridView { get; set; }

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