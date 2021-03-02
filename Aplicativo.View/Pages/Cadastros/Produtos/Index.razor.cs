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

namespace Aplicativo.View.Pages.Cadastros.Produtos
{
    public class ListProdutoPage : HelpComponent
    {

        protected ListItemViewLayout<Produto> ListItemViewLayout { get; set; }
        protected ViewProduto ViewProduto { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

                ListItemViewLayout.Filtros = new List<HelpFiltro>() 
                {
                    HelpViewFiltro.HelpFiltro("Descrição", "Descricao", FiltroType.TextBox),
                };

                await ListItemViewLayout.BtnPesquisar_Click();

            }
        }

        protected async Task ViewLayout_Pesquisar()
        {
            var Request = new Request();
            Request.Parameters.Add(new Parameters("Filtro", ListItemViewLayout.Filtros));
            ListItemViewLayout.ListItemView = await HelpHttp.Send<List<Produto>>(Http, "api/Produto/GetAll", Request);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewProduto.EditItemViewLayout.Carregar((Produto)args);
            ViewProduto.EditItemViewLayout.ViewModal.Show();
        }

        protected async Task ViewLayout_Delete(object args)
        {
            var Request = new Request();
            Request.Parameters.Add(new Parameters("Produtos", ((List<Produto>)args).Select(c => c.ProdutoID)));
            await HelpHttp.Send(Http, "api/Produto/Delete", Request);
        }
    }
}