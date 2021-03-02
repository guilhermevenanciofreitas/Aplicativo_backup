using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Material.Icon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Produtos
{
    public class ViewProdutoPage : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout<Produto> ListItemViewLayout { get; set; }
        public EditItemViewLayout<Produto> EditItemViewLayout { get; set; }


        protected TabSet TabSet { get; set; }

        protected TextBox TxtCodigo { get; set; }
        protected TextBox TxtDescricao { get; set; }
        

        protected DropDownList DplOrigem { get; set; }


        protected ViewProdutoFornecedor ViewProdutoFornecedor { get; set; }


        private Produto Produto = new Produto();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

                DplOrigem.Add("9", "[Selecione]");
                DplOrigem.Add("0", "0 - Nacional");
                DplOrigem.Add("1", "1 - Estrangeira (Importação direta)");
                DplOrigem.Add("2", "2 - Estrangeira (Adquirida no mercado interno)");

                

                //EditItemViewLayout.ItemViewButtons.Add(new ItemViewButton() { Icon = new FilterListIcon(), Label = "Teste", OnClick = Teste });

            }
        }

        protected async Task ViewLayout_Limpar()
        {

            Produto = new Produto();

            //Principal
            TxtCodigo.Text = null;
            TxtDescricao.Text = null;

            //Tributação
            DplOrigem.SelectedValue = "9";

            //Fornecedores
            ViewProdutoFornecedor.ListItemViewLayout.ListItemView = new List<ProdutoFornecedor>();
            ViewProdutoFornecedor.ListItemViewLayout.Refresh();


            TxtDescricao.Focus();

            await TabSet.Active("Principal");

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Request = new Request();

            Request.Parameters.Add(new Parameters("ProdutoID", ((Produto)args)?.ProdutoID));

            Produto = await HelpHttp.Send<Produto>(Http, "api/Produto/Get", Request);


            //Principal
            TxtCodigo.Text = Produto.Codigo.ToStringOrNull();
            TxtDescricao.Text = Produto.Descricao.ToStringOrNull();

            //Tributação
            DplOrigem.SelectedValue = Produto.Origem.ToStringOrNull() ?? "9";

            //Fornecedores
            ViewProdutoFornecedor.ListItemViewLayout.ListItemView = Produto.ProdutoFornecedor.ToList();
            ViewProdutoFornecedor.ListItemViewLayout.Refresh();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtDescricao.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe o nome!", TxtDescricao.Element);
            }


            //Principal
            Produto.Codigo = TxtCodigo.Text.ToStringOrNull();
            Produto.Descricao = TxtDescricao.Text.ToStringOrNull();

            //Tributação
            Produto.Origem = DplOrigem.SelectedValue.ToIntOrNull();

            //Fornecedores
            Produto.ProdutoFornecedor = ViewProdutoFornecedor.ListItemViewLayout.ListItemView;


            var Request = new Request();

            var Produtos = new List<Produto> { Produto };

            Request.Parameters.Add(new Parameters("Produtos", Produtos));

            Produtos = await HelpHttp.Send<List<Produto>>(Http, "api/Produto/Save", Request);

            Produto = Produtos.FirstOrDefault();

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                await EditItemViewLayout.Carregar(Produto);
            }
            else
            {
                EditItemViewLayout.ViewModal.Hide();
            }
            

        }

        protected async Task ViewLayout_Excluir()
        {

            var Request = new Request();

            var Produtos = new List<int?> { Produto.ProdutoID };

            Request.Parameters.Add(new Parameters("Produtos", Produtos));

            await HelpHttp.Send(Http, "api/Produto/Delete", Request);

        }
    }
}