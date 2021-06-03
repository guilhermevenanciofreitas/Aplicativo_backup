using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Vendas
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout<PedidoVenda> ListView { get; set; }
        protected ViewPedidoVenda View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddInclude("Cliente");
            Query.AddInclude("Vendedor");

            ListView.Items = await Query.ToList();
            
        }

        protected async Task BtnItemView_Click(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task BtnExcluir_Click(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<PedidoVenda>().Select(c => (int)c.PedidoVendaID).ToList());
        }

    }
}