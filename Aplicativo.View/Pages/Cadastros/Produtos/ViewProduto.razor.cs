using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Produtos
{
    public partial class ViewProdutoPage : ComponentBase
    {

        public int? ProdutoID { get; set; } = null;

        public Produto ViewModel = new Produto();

        [Parameter] public ListItemViewLayout<Produto> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }

        public DropDownList DplUnidadeMedida { get; set; }

        public DropDownList DplOrigem { get; set; }

        public ViewProdutoFornecedor ViewProdutoFornecedor { get; set; }
        #endregion

        private void InitializeComponents()
        {

            DplOrigem.Items.Clear();
            DplOrigem.Add(null, "[Selecione]");
            DplOrigem.Add("0", "0 - Nacional");
            DplOrigem.Add("1", "1 - Estrangeira (Importação direta)");
            DplOrigem.Add("2", "2 - Estrangeira (Adquirida no mercado interno)");

            DplUnidadeMedida.LoadDropDownList("UnidadeMedidaID", "Descricao", new DropDownListItem(null, "[Selecione]"), HelpParametros.Parametros.UnidadeMedida);

        }

        protected async Task Page_Load(object args)
        {

            InitializeComponents();

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<Produto>();

            Query.AddInclude("ProdutoFornecedor");
            Query.AddInclude("ProdutoFornecedor.Fornecedor");
            Query.AddWhere("ProdutoID == @0", ((Produto)args).ProdutoID);

            ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            ProdutoID = ViewModel.ProdutoID;
            TxtCodigo.Text = ViewModel.Codigo.ToStringOrNull();
            TxtDescricao.Text = ViewModel.Descricao.ToStringOrNull();

            DplUnidadeMedida.SelectedValue = ViewModel.UnidadeMedidaID.ToStringOrNull();

            //Tributação
            DplOrigem.SelectedValue = ViewModel.Origem.ToStringOrNull();

            //Fornecedores
            ViewProdutoFornecedor.ListView.Items = ViewModel.ProdutoFornecedor.ToList();

        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            ProdutoID = null;

            EditItemViewLayout.LimparCampos(this);

            ViewProdutoFornecedor.ListView.Items = new List<ProdutoFornecedor>();

            await TabSet.Active("Principal");

            TxtDescricao.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            if (string.IsNullOrEmpty(TxtDescricao.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe a descrição!", TxtDescricao.Element);
            }

            //Principal
            ViewModel.ProdutoID = ProdutoID;
            ViewModel.Codigo = TxtCodigo.Text.ToStringOrNull();
            ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

            ViewModel.UnidadeMedidaID = DplUnidadeMedida.SelectedValue.ToIntOrNull();

            //Tributação
            ViewModel.Origem = DplOrigem.SelectedValue.ToIntOrNull();

            //Fornecedores
            ViewModel.ProdutoFornecedor = ViewProdutoFornecedor.ListView.Items.ToList();

            foreach(var item in ViewModel.ProdutoFornecedor)
            {
                item.Fornecedor = null;
            }

            var Query = new HelpQuery<Produto>();

            ViewModel = await Query.Update(ViewModel);

            await App.JSRuntime.InvokeVoidAsync("alert", "Salvo com sucesso!!");

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                await Page_Load(ViewModel);
            }
            else
            {
                await EditItemViewLayout.ViewModal.Hide();
            }

        }

        protected async Task BtnExcluir_Click()
        {

            await Excluir(new List<int> { TxtCodigo.Text.ToInt() });

            await EditItemViewLayout.ViewModal.Hide();

        }

        public async Task Excluir(List<int> args)
        {

            var Query = new HelpQuery<Produto>();

            Query.AddWhere("ProdutoID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            await Query.Update(ViewModel, false);

        }

    }
}