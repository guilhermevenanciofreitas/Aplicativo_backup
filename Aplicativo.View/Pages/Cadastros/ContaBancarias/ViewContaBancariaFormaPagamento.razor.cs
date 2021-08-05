using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.ContaBancarias
{
    public class ViewContaBancariaFormaPagamentoPage : ComponentBase
    {

        public ContaBancariaFormaPagamento ViewModel { get; set; } = new ContaBancariaFormaPagamento();

        public ListItemViewLayout<ContaBancariaFormaPagamento> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public ViewPesquisa<FormaPagamento> ViewPesquisaFormaPagamento { get; set; }

        public DropDownList DplTipoTaxa { get; set; }
        public NumericBox TxtTaxa { get; set; }

        public DropDownList DplTipoJuros { get; set; }
        public NumericBox TxtJuros { get; set; }

        public DropDownList DplTipoMulta { get; set; }
        public NumericBox TxtMulta { get; set; }
        #endregion

        #region ListView
        protected void Page_Load()
        {
            ViewPesquisaFormaPagamento.AddWhere("Ativo == @0", true);

            DplTipoTaxa.Items.Clear();
            DplTipoTaxa.Add(null, "[Selecione]");
            DplTipoTaxa.Add(((int)TipoCobranca.Percentual).ToString(), "Percentual");
            DplTipoTaxa.Add(((int)TipoCobranca.ValorFixo).ToString(), "Valor fixo");

            DplTipoJuros.Items.Clear();
            DplTipoJuros.Add(null, "[Selecione]");
            DplTipoJuros.Add(((int)TipoCobranca.Percentual).ToString(), "Percentual");
            DplTipoJuros.Add(((int)TipoCobranca.ValorFixo).ToString(), "Valor fixo");

            DplTipoMulta.Items.Clear();
            DplTipoMulta.Add(null, "[Selecione]");
            DplTipoMulta.Add(((int)TipoCobranca.Percentual).ToString(), "Percentual");
            DplTipoMulta.Add(((int)TipoCobranca.ValorFixo).ToString(), "Valor fixo");


        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void BtnLimpar_Click()
        {

            ViewModel = new ContaBancariaFormaPagamento();

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaFormaPagamento.Clear();

            ViewPesquisaFormaPagamento.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            BtnLimpar_Click();

            if (args == null) return;

            ViewModel = (ContaBancariaFormaPagamento)args;

            ViewPesquisaFormaPagamento.Value = ViewModel.FormaPagamento?.FormaPagamentoID.ToStringOrNull();
            ViewPesquisaFormaPagamento.Text = ViewModel.FormaPagamento?.Descricao.ToStringOrNull();

            DplTipoTaxa.SelectedValue = ((int?)ViewModel.TipoTaxa).ToStringOrNull();
            DplTipoTaxa_Change();
            TxtTaxa.Value = ViewModel.Taxa ?? 0;

            DplTipoJuros.SelectedValue = ((int?)ViewModel.TipoJuros).ToStringOrNull();
            DplTipoJuros_Change();
            TxtJuros.Value = ViewModel.Juros ?? 0;

            DplTipoMulta.SelectedValue = ((int?)ViewModel.TipoMulta).ToStringOrNull();
            DplTipoMulta_Change();
            TxtMulta.Value = ViewModel.Multa ?? 0;

            

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            ViewModel.FormaPagamentoID = ViewPesquisaFormaPagamento.Value.ToIntOrNull();
            ViewModel.FormaPagamento = new FormaPagamento() { Descricao = ViewPesquisaFormaPagamento.Text };

            ViewModel.TipoTaxa = (TipoCobranca?)DplTipoTaxa.SelectedValue.ToIntOrNull();
            ViewModel.Taxa = TxtTaxa.Value;

            ViewModel.TipoJuros = (TipoCobranca?)DplTipoJuros.SelectedValue.ToIntOrNull();
            ViewModel.Juros = TxtJuros.Value;

            ViewModel.TipoMulta = (TipoCobranca?)DplTipoMulta.SelectedValue.ToIntOrNull();
            ViewModel.Multa = TxtMulta.Value;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<ContaBancariaFormaPagamento>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<ContaBancariaFormaPagamento>().ToList());

        }

        public void Excluir(List<ContaBancariaFormaPagamento> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        protected void DplTipoTaxa_Change()
        {

            if (DplTipoTaxa.SelectedValue.ToIntOrNull() == (int)TipoCobranca.Percentual)
            {
                TxtTaxa.Digits = 3;
            }
            if (DplTipoTaxa.SelectedValue.ToIntOrNull() == (int)TipoCobranca.ValorFixo)
            {
                TxtTaxa.Digits = 2;
            }

            if (DplTipoTaxa.SelectedValue.ToIntOrNull() != null)
            {
                TxtTaxa.Focus();
            }
            
        }

        protected void DplTipoJuros_Change()
        {

            if (DplTipoJuros.SelectedValue.ToIntOrNull() == (int)TipoCobranca.Percentual)
            {
                TxtJuros.Digits = 3;
            }
            if (DplTipoJuros.SelectedValue.ToIntOrNull() == (int)TipoCobranca.ValorFixo)
            {
                TxtJuros.Digits = 2;
            }

            if (DplTipoJuros.SelectedValue.ToIntOrNull() != null)
            {
                TxtJuros.Focus();
            }

        }

        protected void DplTipoMulta_Change()
        {

            if (DplTipoMulta.SelectedValue.ToIntOrNull() == (int)TipoCobranca.Percentual)
            {
                TxtMulta.Digits = 3;
            }
            if (DplTipoMulta.SelectedValue.ToIntOrNull() == (int)TipoCobranca.ValorFixo)
            {
                TxtMulta.Digits = 2;
            }

            if (DplTipoMulta.SelectedValue.ToIntOrNull() != null)
            {
                TxtMulta.Focus();
            }

        }

        #endregion

    }
}