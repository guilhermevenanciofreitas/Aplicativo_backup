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
using Syncfusion.Blazor.Grids;
using System;
using System.Collections;
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
        public CheckBox ChkCombinacao { get; set; }
        public CheckBox ChkComposicao { get; set; }

        public DropDownList DplUnidadeMedida { get; set; }


        public SfGrid<RelBasica> GridViewEstoque { get; set; }
        public List<RelBasica> ListEstoque { get; set; } = new List<RelBasica>();

        public ViewProdutoAtributo ViewProdutoAtributo { get; set; }

        public ViewProdutoCombinacao ViewProdutoCombinacao { get; set; }

        public DropDownList DplOrigem { get; set; }
        public ViewPesquisa<NCM> ViewPesquisaNCM { get; set; }
        public ViewPesquisa<CEST> ViewPesquisaCEST { get; set; }
        public ViewPesquisa<Tributacao> ViewPesquisaTributacao { get; set; }

        public ViewProdutoFornecedor ViewProdutoFornecedor { get; set; }
        #endregion

        private void InitializeComponents()
        {

            DplOrigem.Items.Clear();
            DplOrigem.Add(null, "[Selecione]");
            DplOrigem.Add("0", "0 - Nacional");
            DplOrigem.Add("1", "1 - Estrangeira (Importação direta)");
            DplOrigem.Add("2", "2 - Estrangeira (Adquirida no mercado interno)");

            DplUnidadeMedida.LoadDropDownList("UnidadeMedidaID", "Unidade", new DropDownListItem(null, "[Selecione]"), HelpParametros.Parametros.UnidadeMedida.Where(c => c.Ativo == true).ToList());

        }

        protected async Task Page_Load(object args)
        {

            InitializeComponents();

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<Produto>();

            Query.AddInclude("NCM");
            Query.AddInclude("CEST");
            Query.AddInclude("Tributacao");

            Query.AddInclude("ProdutoAtributo");
            Query.AddInclude("ProdutoAtributo.Atributo");

            Query.AddInclude("ProdutoCombinacao");
            Query.AddInclude("ProdutoCombinacao.ProdutoCombinacaoAtributo");
            Query.AddInclude("ProdutoCombinacao.ProdutoCombinacaoAtributo.Atributo");
            Query.AddInclude("ProdutoCombinacao.ProdutoCombinacaoAtributo.Atributo.Variacao");
            Query.AddInclude("ProdutoCombinacao.EstoqueMovimentoItem");
            Query.AddInclude("ProdutoCombinacao.EstoqueMovimentoItem.EstoqueMovimentoItemEntrada");

            Query.AddInclude("ProdutoFornecedor");
            Query.AddInclude("ProdutoFornecedor.Fornecedor");
            Query.AddInclude("ProdutoFornecedor.UnidadeMedida");

            Query.AddInclude("EstoqueMovimentoItem");
            Query.AddInclude("EstoqueMovimentoItem.EstoqueMovimento");
            Query.AddInclude("EstoqueMovimentoItem.EstoqueMovimento.Estoque");
            Query.AddInclude("EstoqueMovimentoItem.EstoqueMovimentoItemEntrada");


            Query.AddWhere("ProdutoID == @0", ((Produto)args).ProdutoID);

            ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            ProdutoID = ViewModel.ProdutoID;
            TxtCodigo.Text = ViewModel.Codigo.ToStringOrNull();
            TxtDescricao.Text = ViewModel.Descricao.ToStringOrNull();

            ChkCombinacao.Checked = ViewModel.Combinacao ?? false;
            //ChkComposicao.Checked = ViewModel

            DplUnidadeMedida.SelectedValue = ViewModel.UnidadeMedidaID.ToStringOrNull();

            //Atributos
            ViewProdutoAtributo.ListView.Items = ViewModel.ProdutoAtributo.ToList();

            //Combinações
            ViewProdutoCombinacao.ListAtributo = ViewModel.ProdutoAtributo.Select(c => c.Atributo).ToList();
            ViewProdutoCombinacao.ListView.Items = ViewModel.ProdutoCombinacao.ToList();

            //Tributação
            DplOrigem.SelectedValue = ViewModel.Origem.ToStringOrNull();

            ViewPesquisaNCM.Value = ViewModel.Codigo_NCM;
            ViewPesquisaNCM.Text = ViewModel.NCM?.Descricao;

            ViewPesquisaCEST.Value = ViewModel.Codigo_CEST;
            ViewPesquisaCEST.Text = ViewModel.CEST?.Descricao;

            ViewPesquisaTributacao.Value = ViewModel.TributacaoID.ToStringOrNull();
            ViewPesquisaTributacao.Text = ViewModel.Tributacao?.Descricao.ToStringOrNull();

            //Fornecedores
            ViewProdutoFornecedor.ListView.Items = ViewModel.ProdutoFornecedor.ToList();


            foreach (var item in ViewModel.EstoqueMovimentoItem)
            {

                var RelBasica = ListEstoque.FirstOrDefault(c => c.Inteiro01 == item.EstoqueMovimento.EstoqueID);

                if (RelBasica == null)
                {
                    ListEstoque.Add(new RelBasica()
                    {
                        Inteiro01 = item.EstoqueMovimento.EstoqueID,
                        Descricao01 = item.EstoqueMovimento.Estoque.Descricao,
                        Valor01 = item.EstoqueMovimentoItemEntrada.Saldo,
                    });
                }
                else
                {
                    RelBasica.Valor01 += item.EstoqueMovimentoItemEntrada.Saldo;
                }


            }

            GridViewEstoque.Refresh();

            //var EstoqueMovimentoItemEntrada = ViewModel.EstoqueMovimentoItem.Select(c => c.EstoqueMovimentoItemEntrada).ToList();



        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            ProdutoID = null;

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaNCM.Clear();
            ViewPesquisaCEST.Clear();
            ViewPesquisaTributacao.Clear();

            ListEstoque.Clear();
            GridViewEstoque.Refresh();


            ViewProdutoAtributo.ListView.Items = new List<ProdutoAtributo>();

            ViewProdutoCombinacao.ListAtributo.Clear();
            ViewProdutoCombinacao.ListView.Items = new List<ProdutoCombinacao>();

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

            ViewModel.Combinacao = ChkCombinacao.Checked;


            ViewModel.UnidadeMedidaID = DplUnidadeMedida.SelectedValue.ToIntOrNull();


            //Atributos
            ViewModel.ProdutoAtributo = ViewProdutoAtributo.ListView.Items.ToList();

            ViewModel.ProdutoCombinacao = ViewProdutoCombinacao.ListView.Items.ToList();

            //Tributação
            ViewModel.Origem = DplOrigem.SelectedValue.ToIntOrNull();

            ViewModel.Codigo_NCM = ViewPesquisaNCM.Value;
            ViewModel.NCM = null;

            ViewModel.Codigo_CEST = ViewPesquisaCEST.Value;
            ViewModel.CEST = null;

            ViewModel.TributacaoID = ViewPesquisaTributacao.Value.ToIntOrNull();
            ViewModel.Tributacao = null;


            //Fornecedores
            ViewModel.ProdutoFornecedor = ViewProdutoFornecedor.ListView.Items.ToList();

            foreach(var item in ViewModel.ProdutoFornecedor)
            {
                item.Fornecedor = null;
                item.UnidadeMedida = null;
            }

            foreach (var item in ViewModel.ProdutoAtributo)
            {
                item.Atributo = null;
            }

            foreach(var item in ViewModel.ProdutoCombinacao)
            {
                foreach(var atributo in item.ProdutoCombinacaoAtributo)
                {
                    atributo.Atributo = null;
                    atributo.Variacao = null;
                }
            }

            ViewModel.EstoqueMovimentoItem = null;

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            var Changes = await HelpUpdate.SaveChanges();

            ViewModel = HelpUpdate.Bind<Produto>(Changes[0]);

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

        protected void ViewProdutoAtributo_Save(object args)
        {

            var Atributos = ((IEnumerable)args).Cast<Atributo>().ToList();

            ViewProdutoCombinacao.ListAtributo = Atributos;

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

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.AddRange(ViewModel);

            await HelpUpdate.SaveChanges();

        }

    }

}