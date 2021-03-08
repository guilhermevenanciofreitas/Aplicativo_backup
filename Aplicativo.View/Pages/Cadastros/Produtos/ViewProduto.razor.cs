using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Produtos
{
    public partial class ViewProdutoPage<TValue> : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        public EditItemViewLayout<Produto> EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }

        public DropDownList DplOrigem { get; set; }

        public ViewProdutoFornecedor ViewProdutoFornecedor { get; set; }
        #endregion


        protected void ViewLayout_PageLoad()
        {

        }

        private void Initialize()
        {

            DplOrigem.Items.Clear();
            DplOrigem.Add("9", "[Selecione]");
            DplOrigem.Add("0", "0 - Nacional");
            DplOrigem.Add("1", "1 - Estrangeira (Importação direta)");
            DplOrigem.Add("2", "2 - Estrangeira (Adquirida no mercado interno)");

        }

        protected async Task ViewLayout_Limpar()
        {

            Initialize();

            EditItemViewLayout.LimparCampos(this);

            DplOrigem.SelectedValue = "0";

            ViewProdutoFornecedor.ListItemViewLayout.ListItemView = new List<ProdutoFornecedor>();
            ViewProdutoFornecedor.ListItemViewLayout.Refresh();

            TxtDescricao.Focus();

            await TabSet.Active("Principal");

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddInclude("ProdutoFornecedor");
            Query.AddInclude("ProdutoFornecedor.Fornecedor");
            Query.AddWhere("ProdutoID == @0", ((Produto)args)?.ProdutoID);
            Query.AddTake(1);

            EditItemViewLayout.ViewModel = await EditItemViewLayout.Carregar<Produto>(Query);

            //Principal
            TxtCodigo.Text = EditItemViewLayout.ViewModel.Codigo.ToStringOrNull();
            TxtDescricao.Text = EditItemViewLayout.ViewModel.Descricao.ToStringOrNull();

            //Tributação
            DplOrigem.SelectedValue = EditItemViewLayout.ViewModel.Origem.ToStringOrNull() ?? "9";

            //Fornecedores
            ViewProdutoFornecedor.ListItemViewLayout.ListItemView = EditItemViewLayout.ViewModel.ProdutoFornecedor.ToList();
            ViewProdutoFornecedor.ListItemViewLayout.Refresh();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtDescricao.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe a descrição!", TxtDescricao.Element);
            }


            //Principal
            EditItemViewLayout.ViewModel.Codigo = TxtCodigo.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

            //Tributação
            EditItemViewLayout.ViewModel.Origem = DplOrigem.SelectedValue.ToIntOrNull();

            //Fornecedores
            EditItemViewLayout.ViewModel.ProdutoFornecedor = ViewProdutoFornecedor.ListItemViewLayout.ListItemView;


            EditItemViewLayout.ViewModel = await EditItemViewLayout.Update(EditItemViewLayout.ViewModel);


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
                await EditItemViewLayout.Carregar(EditItemViewLayout.ViewModel);
            else
                EditItemViewLayout.ViewModal.Hide();


        }

        protected async Task ViewLayout_Excluir()
        {
            await EditItemViewLayout.Delete(EditItemViewLayout.ViewModel);
        }
    }
}