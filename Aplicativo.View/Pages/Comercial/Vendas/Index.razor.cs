using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Vendas
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout<PedidoVenda> ListView { get; set; }

        protected ViewPedidoVendaAndamento ViewPedidoVendaAndamento { get; set; }
        protected ViewPedidoVenda View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddInclude("PedidoVendaStatus");
            Query.AddInclude("Cliente");
            Query.AddInclude("Vendedor");

            Query.AddWhere("Finalizado == null");

            ListView.Items = await Query.ToList();
            
        }

        protected async Task BtnItemView_Click(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task BtnFinalizar_Click(object args)
        {

            ViewPedidoVendaAndamento.PedidoVendaID = ((IEnumerable)args).Cast<PedidoVenda>().Select(c => c.PedidoVendaID).ToList();

            await ViewPedidoVendaAndamento.EditItemViewLayout.Show(null);

        }

        protected async Task ViewPedidoVendaAndamento_Confirm()
        {

            await ViewPedidoVendaAndamento.EditItemViewLayout.ViewModal.Hide();

            await BtnPesquisar_Click();

        }

        protected async Task ViewPedidoVendaAndamento_Finally()
        {

            await ListView.ListViewBtnPesquisa.BtnPesquisar_Click();

        }

        protected async Task BtnExcluir_Click(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<PedidoVenda>().Select(c => (int)c.PedidoVendaID).ToList());
        }

    }
}