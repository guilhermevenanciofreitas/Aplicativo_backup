using Aplicativo.Utils.Helpers;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Financeiro.TituloDetalhes
{

    public enum Tipo
    {
        Pagar,
        Receber,
    }

    public class IndexPage<TValue> : HelpComponent
    {

        [Parameter] public Tipo Tipo { get; set; }

        [Parameter] public string TitleView { get; set; }

        protected ListItemViewLayout<TValue> ListItemViewLayout { get; set; }

        protected AddTitulo Add { get; set; }
        protected ViewTituloDetalhe View { get; set; }

        protected async Task ViewLayout_PageLoad()
        {
            await ListItemViewLayout.BtnPesquisar_Click();
        }

        protected async Task ViewLayout_Pesquisar()
        {

            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddWhere("Titulo.Ativo == @0", true);

            ListItemViewLayout.ListItemView = await ListItemViewLayout.Pesquisar(Query);

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            if (args == null)
            {
                Add.EditItemViewLayout.ViewModal.Show();
            }
            else
            {
                await View.EditItemViewLayout.Carregar(args);
            }
        }

        protected async Task ViewLayout_Delete(object args)
        {
            await ListItemViewLayout.Delete(args);
        }
    }
}