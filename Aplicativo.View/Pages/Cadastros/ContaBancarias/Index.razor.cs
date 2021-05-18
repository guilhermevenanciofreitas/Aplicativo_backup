using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.ContaBancarias
{
    public class IndexPage : ComponentBase
    {

        protected ListItemViewLayout ListItemViewLayout { get; set; }
        protected ViewContaBancaria View { get; set; }

        protected async Task Component_Load()
        {
            await ViewLayout_Pesquisar();
        }

        protected async Task ViewLayout_Pesquisar()
        {

            var Query = new HelpQuery<ContaBancaria>();

            Query.AddWhere("Ativo == @0", true);

            ListItemViewLayout.ListItemView = (await Query.ToList()).Cast<object>().ToList();

            await HelpLoading.Hide();

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task ViewLayout_Excluir(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<Usuario>().Select(c => (int)c.UsuarioID).ToList());
        }

        //protected ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        //protected ViewContaBancaria View { get; set; }

        //protected async Task ViewLayout_PageLoad()
        //{

        //    ListItemViewLayout.Filtros = new List<HelpFiltro>()
        //        {
        //            HelpViewFiltro.HelpFiltro("Descrição", "Nome", FiltroType.TextBox),
        //        };

        //    View.EditItemViewLayout.ItemViewButtons = ListItemViewLayout.ItemViewButtons;

        //    await ListItemViewLayout.BtnPesquisar_Click();

        //}

        //protected async Task ViewLayout_Pesquisar()
        //{

        //    var Query = new HelpQuery<TValue>();

        //    Query.AddWhere("Ativo == @0", true);

        //    ListItemViewLayout.ListItemView = await Query.ToList();

        //}

        //protected async Task ViewLayout_ItemView(object args)
        //{
        //    await View.EditItemViewLayout.Carregar(args);
        //}

        //protected async Task ViewLayout_Delete(object args)
        //{
        //    await ListItemViewLayout.Delete(args);
        //}
    }
}