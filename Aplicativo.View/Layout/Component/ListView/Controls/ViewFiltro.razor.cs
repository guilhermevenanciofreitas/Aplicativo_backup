using Aplicativo.Utils.Helpers;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ListView.Controls
{

    public class ViewFiltroComponent : ComponentBase
    {

        [CascadingParameter] public ListViewBtnFiltro ListViewBtnFiltro { get; set; }

        public ViewModal ViewModal;

        public List<HelpFiltro> Filtros { get; set; } = new List<HelpFiltro>();


        [Parameter] public string Title { get; set; } = "Filtro";

        [Parameter] public EventCallback OnConfirmar { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                ListViewBtnFiltro.ViewModal = ViewModal;
            }
            
        }

        protected async Task ViewFiltro_Confirmar()
        {
            try
            {

                ViewModal.Hide();

                await OnConfirmar.InvokeAsync(null);

                await ListViewBtnFiltro.ListViewBtnPesquisa.BtnPesquisar_Click();

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}
