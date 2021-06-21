using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Fiscal.Tributacoes
{
    public partial class ViewTributacaoPage : ComponentBase
    {

        public Tributacao ViewModel = new Tributacao();

        [Parameter] public ListItemViewLayout<Tributacao> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }

        public TabSet TabCST { get; set; }

        public ViewPesquisa<CST_ICMS> ViewPesquisaICMS { get; set; }
        public ViewPesquisa<CSOSN_ICMS> ViewPesquisaCSOSN { get; set; }
        public NumericBox TxtAliq_ICMS { get; set; }

        public ViewPesquisa<CST_IPI> ViewPesquisaIPI { get; set; }
        public NumericBox TxtAliq_IPI { get; set; }

        public ViewPesquisa<CST_PISCOFINS> ViewPesquisaPIS { get; set; }
        public NumericBox TxtAliq_PIS { get; set; }

        public ViewPesquisa<CST_PISCOFINS> ViewPesquisaCOFINS { get; set; }
        public NumericBox TxtAliq_COFINS { get; set; }

        public ViewTributacaoOperacao ViewTributacaoOperacao { get; set; }

        #endregion

        protected async Task Page_Load(object args)
        {

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<Tributacao>();

            Query.AddInclude("CST_ICMS");
            Query.AddInclude("CSOSN_ICMS");
            Query.AddInclude("CST_IPI");
            Query.AddInclude("CST_PIS");
            Query.AddInclude("CST_COFINS");

            Query.AddInclude("TributacaoOperacao");
            Query.AddInclude("TributacaoOperacao.Operacao");
            Query.AddInclude("TributacaoOperacao.CFOP_Estadual");
            Query.AddInclude("TributacaoOperacao.CFOP_Interestadual");
            Query.AddInclude("TributacaoOperacao.CFOP_Exterior");

            Query.AddWhere("TributacaoID == @0", ((Tributacao)args).TributacaoID);
            
            ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            TxtCodigo.Text = ViewModel.TributacaoID.ToStringOrNull();
            TxtDescricao.Text = ViewModel.Descricao.ToStringOrNull();

            ViewPesquisaICMS.Value = ViewModel.Codigo_CST.ToStringOrNull();
            ViewPesquisaICMS.Text = ViewModel.CST_ICMS?.Descricao.ToStringOrNull();
            ViewPesquisaCSOSN.Value = ViewModel.Codigo_CSOSN.ToStringOrNull();
            ViewPesquisaCSOSN.Text = ViewModel.CSOSN_ICMS?.Descricao.ToStringOrNull();
            TxtAliq_ICMS.Value = ViewModel.Aliq_ICMS ?? 0;

            ViewPesquisaIPI.Value = ViewModel.Codigo_IPI.ToStringOrNull();
            ViewPesquisaIPI.Text = ViewModel.CST_IPI?.Descricao.ToStringOrNull();
            TxtAliq_IPI.Value = ViewModel.Aliq_IPI ?? 0;

            ViewPesquisaPIS.Value = ViewModel.Codigo_PIS.ToStringOrNull();
            ViewPesquisaPIS.Text = ViewModel.CST_PIS?.Descricao.ToStringOrNull();
            TxtAliq_PIS.Value = ViewModel.Aliq_PIS ?? 0;

            ViewPesquisaCOFINS.Value = ViewModel.Codigo_COFINS.ToStringOrNull();
            ViewPesquisaCOFINS.Text = ViewModel.CST_COFINS?.Descricao.ToStringOrNull();
            TxtAliq_COFINS.Value = ViewModel.Aliq_COFINS ?? 0;

            ViewTributacaoOperacao.ListView.Items = ViewModel.TributacaoOperacao.ToList();

        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaICMS.Clear();
            ViewPesquisaCSOSN.Clear();
            ViewPesquisaIPI.Clear();
            ViewPesquisaPIS.Clear();
            ViewPesquisaCOFINS.Clear();


            ViewTributacaoOperacao.ListView.Items = new List<TributacaoOperacao>();

            await TabSet.Active("Principal");
            await TabCST.Active("ICMS");

            TxtDescricao.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            ViewModel.TributacaoID = TxtCodigo.Text.ToIntOrNull();
            ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

            ViewModel.Codigo_CST = ViewPesquisaICMS.Value.ToStringOrNull();
            ViewModel.CST_ICMS = null;
            ViewModel.Codigo_CSOSN = ViewPesquisaCSOSN.Value.ToStringOrNull();
            ViewModel.CSOSN_ICMS = null;
            ViewModel.Aliq_ICMS = TxtAliq_ICMS.Value;

            ViewModel.Codigo_IPI = ViewPesquisaIPI.Value.ToStringOrNull();
            ViewModel.CST_IPI = null;
            ViewModel.Aliq_IPI = TxtAliq_IPI.Value;

            ViewModel.Codigo_PIS = ViewPesquisaPIS.Value.ToStringOrNull();
            ViewModel.CST_PIS = null;
            ViewModel.Aliq_PIS = TxtAliq_PIS.Value;

            ViewModel.Codigo_COFINS = ViewPesquisaCOFINS.Value.ToStringOrNull();
            ViewModel.CST_COFINS = null;
            ViewModel.Aliq_COFINS = TxtAliq_COFINS.Value;

            ViewModel.TributacaoOperacao = ViewTributacaoOperacao.ListView.Items.ToList();

            foreach (var item in ViewModel.TributacaoOperacao)
            {
                item.Operacao = null;
                item.CFOP_Estadual = null;
                item.CFOP_Interestadual = null;
                item.CFOP_Exterior = null;
            }

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            var Changes = await HelpUpdate.SaveChanges();

            ViewModel = HelpUpdate.Bind<Tributacao>(Changes[0]);

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

            var Query = new HelpQuery<Tributacao>();

            Query.AddWhere("TributacaoID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            await HelpUpdate.SaveChanges();

        }

    }
}