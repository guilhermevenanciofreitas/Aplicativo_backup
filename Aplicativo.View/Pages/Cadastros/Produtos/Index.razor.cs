using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Produtos
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout ListItemViewLayout { get; set; }
        protected ViewProduto View { get; set; }

        protected async Task Component_Load()
        {
            await ViewLayout_Pesquisar();
        }

        protected async Task ViewLayout_Pesquisar()
        {

            var Query = new HelpQuery<Produto>();

            Query.AddWhere("Ativo == @0", true);

            ListItemViewLayout.ListItemView = (await Query.ToList()).Cast<object>().ToList();

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task ViewLayout_Excluir(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<Produto>().Select(c => (int)c.ProdutoID).ToList());
        }

    }
}


//using Aplicativo.Utils;
//using Aplicativo.Utils.Helpers;
//using Aplicativo.Utils.Models;
//using Aplicativo.View.Helpers;
//using Aplicativo.View.Layout;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using Skclusive.Material.Icon;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Aplicativo.View.Pages.Cadastros.Produtos
//{
//    public class IndexPage<TValue> : ComponentBase
//    {

//        protected ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
//        protected ViewProduto View { get; set; }

//        protected async Task ViewLayout_PageLoad()
//        {

//            ListItemViewLayout.Filtros = new List<HelpFiltro>()
//                {
//                    HelpViewFiltro.HelpFiltro("Descrição", "Descricao", FiltroType.TextBox),
//                };

//            View.EditItemViewLayout.ItemViewButtons = ListItemViewLayout.ItemViewButtons;

//            await ListItemViewLayout.BtnPesquisar_Click();

//        }

//        protected async Task ViewLayout_Pesquisar()
//        {

//            var Query = new HelpQuery<TValue>();

//            Query.AddWhere("Ativo == @0", true);

//            ListItemViewLayout.ListItemView = await Query.ToList();

//        }

//        protected async Task ViewLayout_ItemView(object args)
//        {
//            await View.EditItemViewLayout.Carregar(args);
//        }

//        protected async Task ViewLayout_Delete(object args)
//        {
//            await ListItemViewLayout.Delete(args);
//        }

//        //protected ListItemViewLayout<Produto> ListItemViewLayout { get; set; }
//        //protected ViewProduto ViewProduto { get; set; }

//        //protected override async Task OnAfterRenderAsync(bool firstRender)
//        //{
//        //    await base.OnAfterRenderAsync(firstRender);

//        //    if (firstRender)
//        //    {

//        //        ListItemViewLayout.Filtros = new List<HelpFiltro>() 
//        //        {
//        //            HelpViewFiltro.HelpFiltro("Descrição", "Descricao", FiltroType.TextBox),
//        //        };

//        //        await ListItemViewLayout.BtnPesquisar_Click();

//        //    }
//        //}

//        //protected async Task ViewLayout_Pesquisar()
//        //{
//        //    var Request = new Request();
//        //    Request.Parameters.Add(new Parameters("Filtro", ListItemViewLayout.Filtros));
//        //    ListItemViewLayout.ListItemView = await HelpHttp.Send<List<Produto>>(Http, "api/Produto/GetAll", Request);
//        //}

//        //protected async Task ViewLayout_ItemView(object args)
//        //{
//        //    await ViewProduto.EditItemViewLayout.Carregar((Produto)args);
//        //    ViewProduto.EditItemViewLayout.ViewModal.Show();
//        //}

//        //protected async Task ViewLayout_Delete(object args)
//        //{
//        //    var Request = new Request();
//        //    Request.Parameters.Add(new Parameters("Produtos", ((List<Produto>)args).Select(c => c.ProdutoID)));
//        //    await HelpHttp.Send(Http, "api/Produto/Delete", Request);
//        //}
//    }
//}