using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ListView.Controls;
using Aplicativo.View.Pages.Comercial.Vendas;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Separacao
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout<PedidoVenda> ListViewPendente { get; set; }

        protected ListViewGridView<PedidoVenda> GridViewEmSeparacao { get; set; }
        protected ListViewGridView<PedidoVenda> GridViewSeparado { get; set; }

        protected ViewPedidoVendaAndamento ViewPedidoVendaAndamentoPendente { get; set; }
        protected ViewPedidoVendaAndamento ViewPedidoVendaAndamentoEmSeparacao { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            var QueryPendente = new HelpQuery<PedidoVenda>();

            QueryPendente.AddInclude("Cliente");
            QueryPendente.AddInclude("Vendedor");

            QueryPendente.AddWhere("Finalizado != null");
            QueryPendente.AddWhere("EmSeparacao == null");
            QueryPendente.AddWhere("Separado == null");

            ListViewPendente.Items = await QueryPendente.ToList();



            var QueryEmSeparacao = new HelpQuery<PedidoVenda>();

            QueryEmSeparacao.AddInclude("Cliente");
            QueryEmSeparacao.AddInclude("Vendedor");

            QueryEmSeparacao.AddWhere("Finalizado != null");
            QueryEmSeparacao.AddWhere("EmSeparacao != null");
            QueryEmSeparacao.AddWhere("Separado == null");

            GridViewEmSeparacao.ListItemView = await QueryEmSeparacao.ToList();



            var QuerySeparado = new HelpQuery<PedidoVenda>();

            QuerySeparado.AddInclude("Cliente");
            QuerySeparado.AddInclude("Vendedor");

            QuerySeparado.AddWhere("Finalizado != null");
            QuerySeparado.AddWhere("EmSeparacao != null");
            QuerySeparado.AddWhere("Separado != null");

            GridViewSeparado.ListItemView = await QuerySeparado.ToList();


        }

        protected async Task BtnItemView_Click(object args)
        {

            ViewPedidoVendaAndamentoPendente.PedidoVendaID = new List<int?>() { ((PedidoVenda)args).PedidoVendaID };

            await ViewPedidoVendaAndamentoPendente.EditItemViewLayout.Show(null);

        }

        protected async Task BtnSeparar_Click(object args)
        {

            ViewPedidoVendaAndamentoPendente.PedidoVendaID = ((IEnumerable)args).Cast<PedidoVenda>().Select(c => c.PedidoVendaID).ToList();

            await ViewPedidoVendaAndamentoPendente.EditItemViewLayout.Show(null);

        }

        protected async Task ViewPedidoVendaAndamentoPendente_Confirm()
        {

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddWhere("PedidoVendaID IN (" + string.Join(",", ViewPedidoVendaAndamentoPendente.PedidoVendaID.ToArray()) + ")");

            var PedidoVenda = await Query.ToList();

            var HelpUpdate = new HelpUpdate();

            foreach(var item in PedidoVenda)
            {
                item.EmSeparacao = DateTime.Now;
                HelpUpdate.Add(item);
            }

            await HelpUpdate.SaveChanges();

            await ViewPedidoVendaAndamentoPendente.EditItemViewLayout.ViewModal.Hide();
            
        }

        protected async Task ViewPedidoVendaAndamentoEmSeparacao_Confirm()
        {

            await ViewPedidoVendaAndamentoEmSeparacao.EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPedidoVendaAndamento_Finally()
        {

            await ListViewPendente.ListViewBtnPesquisa.BtnPesquisar_Click();

        }

    }
}