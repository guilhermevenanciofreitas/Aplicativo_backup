using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout ListItemViewLayout { get; set; }
        protected ViewUsuario View { get; set; }

        protected async Task Component_Load()
        {
            await ViewLayout_Pesquisar();
        }

        protected async Task ViewLayout_Pesquisar()
        {

            var Query = new HelpQuery<Usuario>();

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

    }
}