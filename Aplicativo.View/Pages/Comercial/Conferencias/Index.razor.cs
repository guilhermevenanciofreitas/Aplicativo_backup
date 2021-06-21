using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Conferencias
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout<PedidoVenda> ListView { get; set; }
        protected ViewConferencia View { get; set; }

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
            Query.AddWhere("Separado != null");
            Query.AddWhere("Conferido == null");

            ListView.Items = await Query.ToList();
            
        }

        protected async Task BtnItemView_Click(object args)
        {

            await View.EditItemViewLayout.Show(args);

        }

        protected async Task BtnConferir_Click(object args)
        {
            
            await View.EditItemViewLayout.Show(((IEnumerable)args).Cast<PedidoVenda>().FirstOrDefault());

        }

    }
}