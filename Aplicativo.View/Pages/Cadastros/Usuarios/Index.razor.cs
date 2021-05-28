using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout<Usuario> ListView { get; set; }
        protected ViewUsuario View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            var Query = new HelpQuery<Usuario>();

            Query.AddWhere("Ativo == @0", true);

            ListView.Items = await Query.ToList();
            
            await HelpLoading.Hide();

            StateHasChanged();

        }

        protected async Task BtnItemView_Click(object args)
        {
            await View.EditItemViewLayout.Show(args);
            await BtnPesquisar_Click();
        }

        protected async Task BtnExcluir_Click(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<Usuario>().Select(c => (int)c.UsuarioID).ToList());
        }

    }
}