using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.JSInterop;
using Skclusive.Material.Icon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public class ListUsuarioPage : HelpComponent
    {

        protected ListItemViewLayout<Usuario> ListItemViewLayout { get; set; }
        protected ViewUsuario ViewUsuario { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

                ListItemViewLayout.Filtros = new List<HelpFiltro>() 
                {
                    HelpViewFiltro.HelpFiltro("Nome", "Nome", FiltroType.TextBox),
                    HelpViewFiltro.HelpFiltro("Login", "Login", FiltroType.TextBox),
                };

                ListItemViewLayout.ItemViewButtons.Add(new ItemViewButton() { Icon = new FilterListIcon(), Label = "Imprimir", OnClick = Imprimir });
                ListItemViewLayout.ItemViewButtons.Add(new ItemViewButton() { Icon = new FilterListIcon(), Label = "Compartilhar", OnClick = Compartilhar });

                await ListItemViewLayout.BtnPesquisar_Click();

            }
        }

        private async void Imprimir()
        {
            await JSRuntime.InvokeVoidAsync("alert", "Imprimir");
        }

        private async void Compartilhar()
        {
            await JSRuntime.InvokeVoidAsync("alert", "Compartilhar");
        }

        protected async Task ViewLayout_Pesquisar()
        {
            var Request = new Request();
            Request.Parameters.Add(new Parameters("Filtro", ListItemViewLayout.Filtros));
            ListItemViewLayout.ListItemView = await HelpHttp.Send<List<Usuario>>(Http, "api/Usuario/GetAll", Request);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewUsuario.EditItemViewLayout.Carregar((Usuario)args);
            ViewUsuario.EditItemViewLayout.ViewModal.Show();
        }

        protected async Task ViewLayout_Delete(object args)
        {
            var Request = new Request();
            Request.Parameters.Add(new Parameters("Usuarios", ((List<Usuario>)args).Select(c => c.UsuarioID)));
            await HelpHttp.Send(Http, "api/Usuario/Delete", Request);
        }
    }
}