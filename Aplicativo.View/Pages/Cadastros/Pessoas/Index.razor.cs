using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public class IndexPage : ComponentBase
    {

        [Parameter] public string TitleList { get; set; }

        [Parameter] public Tipo Tipo { get; set; }

        [Parameter] public string TitleView { get; set; }

        protected ListItemViewLayout ListItemViewLayout { get; set; }
        protected ViewPessoa View { get; set; }

        protected async Task Component_Load()
        {
            await ViewLayout_Pesquisar();
        }

        protected async Task ViewLayout_Pesquisar()
        {

            var Query = new HelpQuery<Pessoa>();

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

            ListItemViewLayout.ListItemView = (await Query.ToList()).Cast<object>().ToList();

            await HelpLoading.Hide();

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task ViewLayout_Excluir(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<Pessoa>().Select(c => (int)c.PessoaID).ToList());
        }

        //protected ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        //protected ViewPessoa View { get; set; }

        //protected async Task ViewLayout_PageLoad()
        //{

        //    ListItemViewLayout.Filtros = new List<HelpFiltro>()
        //        {
        //            HelpViewFiltro.HelpFiltro("Nome", "Nome", FiltroType.TextBox),
        //            HelpViewFiltro.HelpFiltro("Login", "Login", FiltroType.TextBox),
        //        };

        //    View.EditItemViewLayout.ItemViewButtons = ListItemViewLayout.ItemViewButtons;

        //    await ListItemViewLayout.BtnPesquisar_Click();

        //}

        //protected async Task ViewLayout_Pesquisar()
        //{

        //    var Query = new HelpQuery<TValue>();

        //    Query.AddWhere("Ativo == @0", true);

        //    switch (Tipo)
        //    {
        //        case Tipo.Cliente:
        //            Query.AddWhere("IsCliente == @0", true);
        //            break;
        //        case Tipo.Fornecedor:
        //            Query.AddWhere("IsFornecedor == @0", true);
        //            break;
        //        case Tipo.Transportadora:
        //            Query.AddWhere("IsTransportadora == @0", true);
        //            break;
        //        case Tipo.Funcionario:
        //            Query.AddWhere("IsFuncionario == @0", true);
        //            break;

        //    }


        //    ListItemViewLayout.ListItemView = await Query.ToList();

        //}

        //protected async Task ViewLayout_ItemView(object args)
        //{
        //    await View.EditItemViewLayout.Carregar(args);
        //}

        //protected async Task ViewLayout_Delete(object args)
        //{
        //    await ListItemViewLayout.Delete(args);
        //}
    }
}