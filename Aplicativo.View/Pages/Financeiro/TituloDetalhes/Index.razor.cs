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

namespace Aplicativo.View.Pages.Financeiro.TituloDetalhes
{

    public enum Tipo
    {
        Pagar,
        Receber,
    }

    public class IndexPage : ComponentBase
    {

        [Parameter] public Tipo Tipo { get; set; }

        [Parameter] public string TitleList { get; set; }

        [Parameter] public string TitleView { get; set; }

        protected ListItemViewLayout ListItemViewLayout { get; set; }

        protected AddTitulo Add { get; set; }
        protected ViewTituloDetalhe View { get; set; }

        protected async Task Component_Load()
        {
            await ViewLayout_Pesquisar();
        }

        protected async Task ViewLayout_Pesquisar()
        {

            var Query = new HelpQuery<TituloDetalhe>();

            Query.AddWhere("Titulo.Ativo == @0", true);

            ListItemViewLayout.ListItemView = (await Query.ToList()).Cast<object>().ToList();

            await HelpLoading.Hide();

        }

        protected async Task BtnNovo_Click()
        {
            await Add.EditItemViewLayout.Show(null);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task ViewLayout_Excluir(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<TituloDetalhe>().Select(c => (int)c.TituloDetalheID).ToList());
        }

        //protected ListItemViewLayout<TValue> ListItemViewLayout { get; set; }

        //protected AddTitulo Add { get; set; }
        //protected ViewTituloDetalhe View { get; set; }

        //protected async Task ViewLayout_PageLoad()
        //{
        //    await ListItemViewLayout.BtnPesquisar_Click();
        //}

        //protected async Task ViewLayout_Pesquisar()
        //{

        //    var Query = new HelpQuery<TValue>();

        //    Query.AddWhere("Titulo.Ativo == @0", true);

        //    ListItemViewLayout.ListItemView = await Query.ToList(); //await ListItemViewLayout.Pesquisar(Query);

        //}

        //protected async Task ViewLayout_ItemView(object args)
        //{
        //    if (args == null)
        //    {
        //        Add.EditItemViewLayout.ViewModal.Show();
        //    }
        //    else
        //    {
        //        await View.EditItemViewLayout.Carregar(args);
        //    }
        //}

        //protected async Task ViewLayout_Delete(object args)
        //{
        //    await ListItemViewLayout.Delete(args);
        //}
    }
}