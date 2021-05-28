using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using System.Collections;
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

        protected ListItemViewLayout<Pessoa> ListView { get; set; }
        protected ViewPessoa View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
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

            ListView.Items = await Query.ToList();

        }

        protected async Task BtnItemView_Click(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task BtnExcluir_Click(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<Pessoa>().Select(c => (int)c.PessoaID).ToList());
        }

    }
}