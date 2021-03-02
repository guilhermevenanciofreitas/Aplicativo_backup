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

namespace Aplicativo.View.Pages.Estoque.Requisicao
{
    public class ListRequisicaoPage : HelpComponent
    {

        protected ListItemViewLayout<Utils.Models.Requisicao> ListItemViewLayout { get; set; }
        protected ViewRequisicao ViewRequisicao { get; set; }

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
            ListItemViewLayout.ListItemView = await HelpHttp.Send<List<Utils.Models.Requisicao>>(Http, "api/Requisicao/GetAll", Request);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewRequisicao.EditItemViewLayout.Carregar((Utils.Models.Requisicao)args);
            ViewRequisicao.EditItemViewLayout.ViewModal.Show();
        }

        protected async Task ViewLayout_Delete(object args)
        {
            var Request = new Request();
            Request.Parameters.Add(new Parameters("Requisicoes", ((List<Utils.Models.Requisicao>)args).Select(c => c.RequisicaoID)));
            await HelpHttp.Send(Http, "api/Requisicao/Delete", Request);
        }
    }
}