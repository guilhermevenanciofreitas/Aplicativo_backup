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


        public NumericBox TxtBaseCalculo { get; set; }

        public ViewPesquisa<CST_ICMS> ViewPesquisaCST_ICMS { get; set; }
        public ViewPesquisa<CSOSN_ICMS> ViewPesquisaCSOSN_ICMS { get; set; }
        public NumericBox TxtICMSBaseCalculo { get; set; }
        public NumericBox TxtAliqICMS { get; set; }
        public NumericBox TxtValorICMS { get; set; }

        public ViewPesquisa<CST_IPI> ViewPesquisaCST_IPI { get; set; }
        public NumericBox TxtIPIBaseCalculo { get; set; }
        public NumericBox TxtAliqIPI { get; set; }
        public NumericBox TxtValorIPI { get; set; }

        public ViewPesquisa<CST_PISCOFINS> ViewPesquisaCST_PIS { get; set; }
        public NumericBox TxtPISBaseCalculo { get; set; }
        public NumericBox TxtAliqPIS { get; set; }
        public NumericBox TxtValorPIS { get; set; }

        public ViewPesquisa<CST_PISCOFINS> ViewPesquisaCST_COFINS { get; set; }
        public NumericBox TxtCOFINSBaseCalculo { get; set; }
        public NumericBox TxtAliqCOFINS { get; set; }
        public NumericBox TxtValorCOFINS { get; set; }


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

            ViewPesquisaCST_ICMS.Clear();
            ViewPesquisaCSOSN_ICMS.Clear();
            ViewPesquisaCST_IPI.Clear();
            ViewPesquisaCST_PIS.Clear();
            ViewPesquisaCST_COFINS.Clear();


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


            TxtBaseCalculo.Value = ViewModel.vBC ?? 0;

            ViewPesquisaCST_ICMS.Value = ViewModel.Codigo_CST;
            ViewPesquisaCST_ICMS.Text = ViewModel.CST_ICMS?.Descricao;

            ViewPesquisaCSOSN_ICMS.Value = ViewModel.Codigo_CSOSN;
            ViewPesquisaCSOSN_ICMS.Text = ViewModel.CSOSN_ICMS?.Descricao;

            TxtAliqICMS.Value = ViewModel.pICMS ?? 0;
            TxtValorICMS.Value = ViewModel.vICMS ?? 0;

            ViewPesquisaCST_IPI.Value = ViewModel.Codigo_IPI;
            ViewPesquisaCST_IPI.Text = ViewModel.CST_IPI?.Descricao;
            TxtAliqIPI.Value = ViewModel.pIPI ?? 0;
            TxtValorIPI.Value = ViewModel.vIPI ?? 0;

            ViewPesquisaCST_PIS.Value = ViewModel.Codigo_PIS;
            ViewPesquisaCST_PIS.Text = ViewModel.CST_PIS?.Descricao;
            TxtAliqPIS.Value = ViewModel.pPIS ?? 0;
            TxtValorPIS.Value = ViewModel.vPIS ?? 0;

            ViewPesquisaCST_COFINS.Value = ViewModel.Codigo_COFINS;
            ViewPesquisaCST_COFINS.Text = ViewModel.CST_COFINS?.Descricao;
            TxtAliqCOFINS.Value = ViewModel.pCOFINS ?? 0;
            TxtValorCOFINS.Value = ViewModel.vCOFINS ?? 0;

            TxtICMSBaseCalculo.Value = TxtBaseCalculo.Value;
            TxtIPIBaseCalculo.Value = TxtBaseCalculo.Value;
            TxtPISBaseCalculo.Value = TxtBaseCalculo.Value;
            TxtCOFINSBaseCalculo.Value = TxtCOFINSBaseCalculo.Value;


        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            //if (ViewPesquisaProduto.Value.ToIntOrNull() == null)
            //{
            //    throw new EmptyException("Informe o produto!", ViewPesquisaProduto.Element);
            //}

            ViewModel.cProd = TxtCodigo.Text.ToStringOrNull();
            ViewModel.xProd = TxtDescricao.Text.ToStringOrNull();

            ViewModel.qCom = TxtQuantidade.Value;
            ViewModel.vUnCom = TxtPreco.Value;

            ViewModel.Codigo_CFOP = ViewPesquisaCFOP.Value;
            ViewModel.CFOP = new CFOP() { Descricao = ViewPesquisaCFOP.Text };

            ViewModel.Codigo_NCM = ViewPesquisaNCM.Value;
            ViewModel.NCM = new NCM() { Descricao = ViewPesquisaNCM.Text };

            ViewModel.Codigo_CEST = ViewPesquisaCEST.Value;
            ViewModel.CEST = new CEST() { Descricao = ViewPesquisaCEST.Text };


            ViewModel.vBC = TxtBaseCalculo.Value;

            ViewModel.Codigo_CST = ViewPesquisaCST_ICMS.Value;
            ViewModel.CST_ICMS = new CST_ICMS() { Descricao = ViewPesquisaCST_ICMS.Text };

            ViewModel.Codigo_CSOSN = ViewPesquisaCSOSN_ICMS.Value;
            ViewModel.CSOSN_ICMS = new CSOSN_ICMS() { Descricao = ViewPesquisaCSOSN_ICMS.Text };

            ViewModel.pICMS = TxtAliqICMS.Value;
            ViewModel.vICMS = TxtValorICMS.Value;


            ViewModel.Codigo_IPI = ViewPesquisaCST_IPI.Value;
            ViewModel.CST_IPI = new CST_IPI() { Descricao = ViewPesquisaCST_IPI.Text };
            ViewModel.pIPI = TxtAliqIPI.Value;
            ViewModel.vIPI = TxtValorIPI.Value;

            ViewModel.Codigo_PIS = ViewPesquisaCST_PIS.Value;
            ViewModel.CST_PIS = new CST_PISCOFINS() { Descricao = ViewPesquisaCST_PIS.Text };
            ViewModel.pPIS = TxtAliqPIS.Value;
            ViewModel.vPIS = TxtValorPIS.Value;

            ViewModel.Codigo_COFINS = ViewPesquisaCST_COFINS.Value;
            ViewModel.CST_COFINS = new CST_PISCOFINS() { Descricao = ViewPesquisaCST_COFINS.Text };
            ViewModel.pCOFINS = TxtAliqCOFINS.Value;
            ViewModel.vCOFINS = TxtValorCOFINS.Value;


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

        protected void TxtBaseCalculo_KeyUp()
        {

            TxtICMSBaseCalculo.Value = TxtBaseCalculo.Value;
            TxtIPIBaseCalculo.Value = TxtBaseCalculo.Value;
            TxtPISBaseCalculo.Value = TxtBaseCalculo.Value;
            TxtCOFINSBaseCalculo.Value = TxtBaseCalculo.Value;

            TxtAliqICMS_KeyUp();
            TxtAliqIPI_KeyUp();
            TxtAliqPIS_KeyUp();
            TxtAliqCOFINS_KeyUp();

        }

        protected void TxtAliqICMS_KeyUp()
        {
            TxtValorICMS.Value = TxtBaseCalculo.Value * (TxtAliqICMS.Value / 100);
        }
        protected void TxtAliqIPI_KeyUp()
        {
            TxtValorIPI.Value = TxtBaseCalculo.Value * (TxtAliqIPI.Value / 100);
        }
        protected void TxtAliqPIS_KeyUp()
        {
            TxtValorPIS.Value = TxtBaseCalculo.Value * (TxtAliqPIS.Value / 100);
        }
        protected void TxtAliqCOFINS_KeyUp()
        {
            TxtValorCOFINS.Value = TxtBaseCalculo.Value * (TxtAliqCOFINS.Value / 100);
        }

        #endregion

    }
}