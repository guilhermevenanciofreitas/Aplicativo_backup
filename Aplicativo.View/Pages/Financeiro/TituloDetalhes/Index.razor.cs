using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Financeiro.TituloDetalhes
{

    public enum Tipo
    {
        Pagar = 1,
        Receber = 2,
    }

    public class IndexPage : ComponentBase
    {

        [Parameter] public Tipo Tipo { get; set; }

        [Parameter] public string TitleList { get; set; }

        [Parameter] public string TitleView { get; set; }

        protected ListItemViewLayout<TituloDetalhe> ListView { get; set; }

        protected AddTitulo Add { get; set; }
        protected ViewTituloDetalhe View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            var Query = new HelpQuery<TituloDetalhe>();

            Query.AddInclude("Pessoa");
            Query.AddInclude("ContaBancaria");
            Query.AddInclude("FormaPagamento");
            

            switch (Tipo)
            {
                case Tipo.Receber:
                    Query.AddWhere("PlanoConta.PlanoContaTipoID == @0", 1);
                    break;
                case Tipo.Pagar:
                    Query.AddWhere("PlanoConta.PlanoContaTipoID == @0", 2);
                    break;
            }

            Query.AddWhere("Titulo.Ativo == @0", true);

            ListView.Items = await Query.ToList();

        }

        protected async Task BtnNovo_Click()
        {
            await Add.EditItemViewLayout.Show(null);
        }

        protected async Task BtnItemView_Click(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task BtnBaixar_Click(object args)
        {
            await View.Baixar(((IEnumerable)args).Cast<TituloDetalhe>().Select(c => (int)c.TituloDetalheID).ToList());
        }

    }
}