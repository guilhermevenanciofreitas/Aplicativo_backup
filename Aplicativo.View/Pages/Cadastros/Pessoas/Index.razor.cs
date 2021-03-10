using Aplicativo.Utils.Helpers;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{

    public enum Tipo
    {
        Cliente,
        Fornecedor,
        Transportadora,
        Funcionario,
    }

    public class IndexPage<TValue> : HelpComponent
    {

        [Parameter] public Tipo Tipo { get; set; }

        [Parameter] public string TitleList { get; set; }
        [Parameter] public string TitleView { get; set; }

        protected ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        protected ViewPessoa View { get; set; }

        protected async Task ViewLayout_PageLoad()
        {

            ListItemViewLayout.Filtros = new List<HelpFiltro>()
                {
                    HelpViewFiltro.HelpFiltro("Nome", "Nome", FiltroType.TextBox),
                    HelpViewFiltro.HelpFiltro("Login", "Login", FiltroType.TextBox),
                };

            View.EditItemViewLayout.ItemViewButtons = ListItemViewLayout.ItemViewButtons;

            await ListItemViewLayout.BtnPesquisar_Click();

        }

        protected async Task ViewLayout_Pesquisar()
        {
            
            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddWhere("Ativo == @0", true);

            switch (Tipo)
            {
                case Tipo.Cliente:
                    Query.AddWhere("IsCliente == @0", true);
                    break;
                case Tipo.Fornecedor:
                    Query.AddWhere("IsFornecedor == @0", true);
                    break;
                case Tipo.Transportadora:
                    Query.AddWhere("IsTransportadora == @0", true);
                    break;
                case Tipo.Funcionario:
                    Query.AddWhere("IsFuncionario == @0", true);
                    break;

            }

            
            ListItemViewLayout.ListItemView = await ListItemViewLayout.Pesquisar(Query);

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await View.EditItemViewLayout.Carregar(args);
        }

        protected async Task ViewLayout_Delete(object args)
        {
            await ListItemViewLayout.Delete(args);
        }
    }
}