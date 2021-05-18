//using Aplicativo.Utils.Helpers;
//using Aplicativo.View.Helpers;
//using Aplicativo.View.Layout;
//using Microsoft.AspNetCore.Components;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Aplicativo.View.Pages.Estoque.Requisicoes
//{
//    public class IndexPage<TValue> : ComponentBase
//    {

//        protected ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
//        protected ViewRequisicao View { get; set; }

//        protected async Task ViewLayout_PageLoad()
//        {

//            ListItemViewLayout.Filtros = new List<HelpFiltro>()
//                {

//                };


//            ListItemViewLayout.BtnExcluir.Visible = false;
//            View.EditItemViewLayout.ItemViewButtons = ListItemViewLayout.ItemViewButtons;

//            await ListItemViewLayout.BtnPesquisar_Click();

//        }

//        protected async Task ViewLayout_Pesquisar()
//        {

//            var Query = new HelpQuery<TValue>();

//            //Query.AddWhere("Ativo == @0", true);

//            ListItemViewLayout.ListItemView = await Query.ToList(); ;

//        }

//        protected async Task ViewLayout_ItemView(object args)
//        {
//            await View.EditItemViewLayout.Carregar(args);
//        }

//        protected async Task ViewLayout_Delete(object args)
//        {
//            await ListItemViewLayout.Delete(args);
//        }
//    }
//}