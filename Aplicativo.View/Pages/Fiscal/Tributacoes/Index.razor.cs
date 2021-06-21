using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Fiscal.Tributacoes
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout<Tributacao> ListView { get; set; }
        protected ViewTributacao View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            var Query = new HelpQuery<Tributacao>();

            Query.AddInclude("CST_ICMS");
            Query.AddInclude("CSOSN_ICMS");
            Query.AddInclude("CST_IPI");
            Query.AddInclude("CST_PIS");
            Query.AddInclude("CST_COFINS");

            ListView.Items = await Query.ToList();
            
        }

        protected async Task BtnItemView_Click(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task BtnExcluir_Click(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<Tributacao>().Select(c => (int)c.TributacaoID).ToList());
        }

    }
}