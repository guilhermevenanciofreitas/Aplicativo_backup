using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Configuracao.Empresas
{

    public class IndexPage : ComponentBase
    {

        [Parameter] public string TitleView { get; set; }

        protected ListItemViewLayout<Empresa> ListView { get; set; }
        protected ViewEmpresa View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            var Query = new HelpQuery<Empresa>();

            Query.AddWhere("Ativo == @0", true);

            ListView.Items = await Query.ToList();

        }

        protected async Task BtnItemView_Click(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task BtnExcluir_Click(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<Empresa>().Select(c => (int)c.EmpresaID).ToList());
        }

    }
}