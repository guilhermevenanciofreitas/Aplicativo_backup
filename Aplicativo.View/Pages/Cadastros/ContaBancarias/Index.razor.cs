using Aplicativo.Utils.Helpers;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.ContaBancarias
{
    public class IndexPage<TValue> : HelpComponent
    {

        protected ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        protected ViewContaBancaria View { get; set; }

        protected async Task ViewLayout_PageLoad()
        {

            ListItemViewLayout.Filtros = new List<HelpFiltro>()
                {
                    HelpViewFiltro.HelpFiltro("Descrição", "Nome", FiltroType.TextBox),
                };

            View.EditItemViewLayout.ItemViewButtons = ListItemViewLayout.ItemViewButtons;

            await ListItemViewLayout.BtnPesquisar_Click();

        }

        protected async Task ViewLayout_Pesquisar()
        {
            
            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddWhere("Ativo == @0", true);
            
            ListItemViewLayout.ListItemView = await ListItemViewLayout.Pesquisar(Query);

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await View.EditItemViewLayout.Carregar(args);
        }

        protected async Task ViewLayout_Delete(object args)
        {
            await ListItemViewLayout.Delete(args);
        }
    }
}