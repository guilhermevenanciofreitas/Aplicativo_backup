using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Fiscal.NotasFiscais
{
    public class ViewNotaFiscalItemPage : ComponentBase
    {

        public NotaFiscalItem ViewModel { get; set; } = new NotaFiscalItem();

        public ListItemViewLayout<NotaFiscalItem> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }


        #region Elements

        public TabSet TabPrincipal { get; set; }
        public TabSet TabImpostos { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }
        public NumericBox TxtQuantidade { get; set; }
        public NumericBox TxtPreco { get; set; }
        public NumericBox TxtDesconto { get; set; }
        public NumericBox TxtTotal { get; set; }


        public ViewPesquisa<CFOP> ViewPesquisaCFOP { get; set; }
        public ViewPesquisa<NCM> ViewPesquisaNCM { get; set; }
        public ViewPesquisa<CEST> ViewPesquisaCEST { get; set; }

        #endregion

        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected async Task ViewLayout_Limpar()
        {

            //ViewPesquisaProduto.AddWhere("Ativo == @0", true);

            ViewModel = new NotaFiscalItem();

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaCFOP.Clear();
            ViewPesquisaNCM.Clear();
            ViewPesquisaCEST.Clear();


            await TabPrincipal.Active("Item");
            await TabImpostos.Active("ICMS");

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            await ViewLayout_Limpar();

            if (args == null) return;

            ViewModel = (NotaFiscalItem)args;

            TxtCodigo.Text = ViewModel.cProd;
            TxtDescricao.Text = ViewModel.xProd;

            TxtQuantidade.Value = ViewModel.qCom ?? 0;
            TxtPreco.Value = ViewModel.vUnCom ?? 0;
            TxtDesconto.Value = ViewModel.vDesc ?? 0;
            TxtTotal.Value = ViewModel.vProd ?? 0;


            ViewPesquisaCFOP.Value = ViewModel.Codigo_CFOP;
            ViewPesquisaCFOP.Text = ViewModel.CFOP?.Descricao;

            ViewPesquisaNCM.Value = ViewModel.Codigo_NCM;
            ViewPesquisaNCM.Text = ViewModel.NCM?.Descricao;

            ViewPesquisaCEST.Value = ViewModel.Codigo_CEST;
            ViewPesquisaCEST.Text = ViewModel.CEST?.Descricao;


        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            //if (ViewPesquisaProduto.Value.ToIntOrNull() == null)
            //{
            //    throw new EmptyException("Informe o produto!", ViewPesquisaProduto.Element);
            //}

            ViewModel.cProd = TxtCodigo.Text.ToStringOrNull();
            ViewModel.xProd = TxtDescricao.Text.ToStringOrNull();

            ViewModel.Codigo_CFOP = ViewPesquisaCFOP.Value;
            ViewModel.CFOP = new CFOP() { Descricao = ViewPesquisaCFOP.Text };

            ViewModel.Codigo_NCM = ViewPesquisaNCM.Value;
            ViewModel.NCM = new NCM() { Descricao = ViewPesquisaNCM.Text };

            ViewModel.Codigo_CEST = ViewPesquisaCEST.Value;
            ViewModel.CEST = new CEST() { Descricao = ViewPesquisaCEST.Text };


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<NotaFiscalItem>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<NotaFiscalItem>().ToList());

        }

        public void Excluir(List<NotaFiscalItem> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        #endregion

    }
}