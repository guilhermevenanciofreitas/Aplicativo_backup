using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Model;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.JSInterop;
using Skclusive.Material.Icon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros
{
    public class ListUsuarioPage : HelpComponent
    {

        protected ListItemViewLayout ViewLayout { get; set; }
        protected ViewUsuario ViewUsuario { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                ViewLayout.Filtros = new List<HelpFiltro>() {
                                HelpViewFiltro.HelpFiltro("Nome", "Nome", FiltroType.TextBox),
                                HelpViewFiltro.HelpFiltro("Login", "Login", FiltroType.TextBox),
                             };

                ViewLayout.ItemViewButtons.Add(new ItemViewButton() { Icon = new FilterListIcon(), Label = "Imprimir", OnClick = Imprimir });
                ViewLayout.ItemViewButtons.Add(new ItemViewButton() { Icon = new FilterListIcon(), Label = "Compartilhar", OnClick = Compartilhar });

                await ViewLayout.OnPesquisar.InvokeAsync(null);

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
            Request.Parameters.Add(new Parameters("Filtro", ViewLayout.Filtros));
            var Result = await HelpHttp.Send<List<Usuario>>(Http, "api/Usuario/GetAll", Request);

            ViewLayout.ListItemView = Result.Select(c =>
            new ItemView()
            {
                Bool01 = false,
                Long01 = c.UsuarioID,
                Descricao01 = c.Nome,
                Descricao02 = c.Login,
            }).ToList();

        }

        protected async Task ViewLayout_ItemView(object ID)
        {
            await ViewUsuario.EditItemViewLayout.Carregar(ID?.ToString().ToIntOrNull());
            ViewUsuario.EditItemViewLayout.ViewModal.Show();
        }

        protected async Task ViewLayout_Delete(object List)
        {
            var Request = new Request();
            Request.Parameters.Add(new Parameters("Usuarios", (List<long?>)List));
            await HelpHttp.Send(Http, "api/Usuario/Delete", Request);
        }
    }
}