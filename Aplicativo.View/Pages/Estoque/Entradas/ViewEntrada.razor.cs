using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Estoque.Entradas
{
    public partial class ViewEntradaPage : ComponentBase
    {

        public EstoqueMovimento ViewModel = new EstoqueMovimento();

        [Parameter] public ListItemViewLayout<EstoqueMovimento> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public ViewPesquisa<NotaFiscal> ViewPesquisaNotaFiscal { get; set; }
        public TextBox TxtSerie { get; set; }
        public TextBox TxtChaveAcesso { get; set; }
        public TextBox TxtCNPJ { get; set; }
        public TextBox TxtRazaoSocial { get; set; }
        public TextBox TxtNomeFantasia { get; set; }

        public ViewPesquisa<Utils.Models.Estoque> ViewPesquisaEstoque { get; set; }
        public ViewEntradaItem ViewEntradaItem { get; set; }
        #endregion

        protected async Task Page_Load(object args)
        {

            //ViewPesquisaFuncionario.AddWhere("IsFuncionario == @0", true);
            //ViewPesquisaFuncionario.AddWhere("Ativo == @0", true);

            await BtnLimpar_Click();

            //if (args == null) return;

            //var Query = new HelpQuery<Usuario>();

            //Query.AddInclude("Funcionario");
            //Query.AddInclude("UsuarioEmail");
            //Query.AddWhere("UsuarioID == @0", ((Usuario)args).UsuarioID);
            
            //ViewModel = await Query.FirstOrDefault();

            //EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            //TxtCodigo.Text = ViewModel.UsuarioID.ToStringOrNull();
            //TxtLogin.Text = ViewModel.Login.ToStringOrNull();

            //ViewPesquisaFuncionario.Value = ViewModel.FuncionarioID.ToStringOrNull();
            //ViewPesquisaFuncionario.Text = ViewModel.Funcionario?.NomeFantasia.ToStringOrNull();

            //TxtSenha.Text = ViewModel.Senha.ToStringOrNull();
            //TxtConfirmarSenha.Text = ViewModel.Senha.ToStringOrNull();

            //ViewUsuarioEmail.ListView.Items = ViewModel.UsuarioEmail.ToList();
            
        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaNotaFiscal.Clear();

            ViewEntradaItem.ListView.Items = new List<EstoqueMovimentoItem>();

            await TabSet.Active("NotaFiscal");

            ViewPesquisaNotaFiscal.Focus();

        }

        protected async Task ViewPesquisaNotaFiscal_Change(object args)
        {

            if (args == null) return;

            var NotaFiscal = (NotaFiscal)args;

            TxtSerie.Text = NotaFiscal.serie.ToStringOrNull();
            TxtChaveAcesso.Text = NotaFiscal.chNFe.ToStringOrNull();
            TxtCNPJ.Text = NotaFiscal.CNPJCPF;
            TxtRazaoSocial.Text = NotaFiscal.xNome;
            TxtNomeFantasia.Text = NotaFiscal.xFant;

            var QueryProdutoFornecedor = new HelpQuery<ProdutoFornecedor>();

            QueryProdutoFornecedor.AddInclude("Produto");
            QueryProdutoFornecedor.AddInclude("Produto.ProdutoFornecedor");

            QueryProdutoFornecedor.AddWhere("Fornecedor.CNPJ == @0", NotaFiscal.CNPJCPF);

            var ProdutoFornecedor = await QueryProdutoFornecedor.ToList();

            var HelpQuery = new HelpQuery<NotaFiscal>();

            HelpQuery.AddInclude("NotaFiscalItem");

            var Item = await HelpQuery.FirstOrDefault();

            var EstoqueMovimentoItem = new List<EstoqueMovimentoItem>();

            foreach (var item in Item.NotaFiscalItem)
            {

                var ProdFornecedor = ProdutoFornecedor.FirstOrDefault(c => c.CodigoFornecedor == item.cProd);

                EstoqueMovimentoItem.Add(new EstoqueMovimentoItem()
                {
                    NotaFiscalItemID = item.NotaFiscalItemID,
                    NotaFiscalItem = item,
                    ProdutoID = ProdFornecedor?.ProdutoID,
                    Produto = ProdFornecedor?.Produto,
                    Quantidade = item.qCom,
                });

            }

            ViewEntradaItem.ListView.Items = EstoqueMovimentoItem.ToList();

        }

        protected async Task BtnFinalizar_Click()
        {

            //if (string.IsNullOrEmpty(TxtLogin.Text))
            //{
            //    await TabSet.Active("Principal");
            //    throw new EmptyException("Informe o login!", TxtLogin.Element);
            //}

            ViewModel.EstoqueID = ViewPesquisaEstoque.Value.ToIntOrNull();
            ViewModel.EstoqueMovimentoTipoID = EstoqueMovimentoTipo.Entrada;
            ViewModel.Data = DateTime.Now;
            ViewModel.FuncionarioID = HelpParametros.Parametros.UsuarioLogado.FuncionarioID;

            ViewModel.EstoqueMovimentoItem = ViewEntradaItem.ListView.Items.ToList();

            foreach(var item in ViewModel.EstoqueMovimentoItem)
            {

                gerarNovoCodigoBarra:

                var CodigoBarra = Convert.ToInt64("1" + item.NotaFiscalItemID?.ToString("00000000") + new Random().Next(0, 9999).ToString("0000"));

                var HelpQuery = new HelpQuery<EstoqueMovimentoItemEntrada>();

                HelpQuery.AddWhere("CodigoBarra == @0", CodigoBarra);

                var Existe = await HelpQuery.FirstOrDefault();

                if (Existe != null)
                {
                    goto gerarNovoCodigoBarra;
                }



                item.Produto = null;
                item.NotaFiscalItem = null;
                item.EstoqueMovimentoItemEntrada = new EstoqueMovimentoItemEntrada()
                {
                    CodigoBarra = CodigoBarra,
                    Saldo = item.Quantidade,
                };

            }

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            await HelpUpdate.SaveChanges();

            await EditItemViewLayout.ViewModal.Hide();

        }

        //protected async Task BtnExcluir_Click()
        //{

        //    //await Excluir(new List<int> { TxtCodigo.Text.ToInt() });

        //    await EditItemViewLayout.ViewModal.Hide();

        //}

        //public async Task Excluir(List<int> args)
        //{

        //    var Query = new HelpQuery<EstoqueMovimento>();

        //    Query.AddWhere("EstoqueMovimentoID IN (" + string.Join(",", args.ToArray()) + ")");

        //    var ViewModel = await Query.ToList();

        //    //foreach (var item in ViewModel)
        //    //{
        //    //    item.Ativo = false;
        //    //}

        //    //await Query.Update(ViewModel, false);

        //}


    }
}