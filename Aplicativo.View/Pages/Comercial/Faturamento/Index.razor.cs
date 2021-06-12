using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Faturamento
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout<PedidoVenda> ListView { get; set; }
        protected ViewFaturamento View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddInclude("Cliente");
            Query.AddInclude("Vendedor");

            Query.AddWhere("Finalizado != null");
            Query.AddWhere("Faturamento == null");

            ListView.Items = await Query.ToList();
            
        }

        protected async Task BtnItemView_Click(object args)
        {

            var PedidoVenda = (PedidoVenda)args;

            await View.EditItemViewLayout.Show(new List<int?> { PedidoVenda.PedidoVendaID });

        }

        protected async Task BtnFaturar_Click(object args)
        {
            await View.EditItemViewLayout.Show(((IEnumerable)args).Cast<PedidoVenda>().Select(c => c.PedidoVendaID).ToList());
            //await View.Page_Load(((IEnumerable)args).Cast<PedidoVenda>().Select(c => (int)c.PedidoVendaID).ToList());
        }

    }
}