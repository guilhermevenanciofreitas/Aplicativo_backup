using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Financeiro.TituloDetalhes
{

    public partial class AddTituloPage : ComponentBase
    {

        [Parameter] public Tipo Tipo { get; set; }
        [Parameter] public ListItemViewLayout<TituloDetalhe> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtDocumento { get; set; }
        public DatePicker DtpEmissao { get; set; }

        public ViewPesquisa<Pessoa> ViewPesquisaPessoa { get; set; }

        public NumericBox TxtValor { get; set; }
        public DatePicker DtpVencimento { get; set; }
        public NumericBox TxtParcelas { get; set; }
        public DropDownList DplPeriodo { get; set; }
        public NumericBox TxtPeriodo { get; set; }

        public ViewPesquisa<PlanoConta> ViewPesquisaPlanoConta { get; set; }
        public ViewPesquisa<ContaBancaria> ViewPesquisaContaBancaria { get; set; }
        public ViewPesquisa<FormaPagamento> ViewPesquisaFormaPagamento { get; set; }
        public ViewPesquisa<CentroCusto> ViewPesquisaCentroCusto { get; set; }


        public SfGrid<TituloDetalhe> GridViewTituloDetalhe { get; set; }
        public List<TituloDetalhe> ListTituloDetalhe { get; set; } = new List<TituloDetalhe>();

        #endregion

        protected async Task Page_Load(object args)
        {

            switch (Tipo)
            {
                case Tipo.Pagar:
                    ViewPesquisaPessoa.Label = "Recebedor";
                    ViewPesquisaPessoa.Title = "Procurar recebedor";
                    ViewPesquisaPlanoConta.AddWhere("PlanoContaTipoID == @0", 2);
                    break;
                case Tipo.Receber:
                    ViewPesquisaPessoa.Label = "Pagador";
                    ViewPesquisaPessoa.Title = "Procurar pagador";
                    ViewPesquisaPlanoConta.AddWhere("PlanoContaTipoID == @0", 1);
                    break;
            }

            ViewPesquisaContaBancaria_Change(null);

            await BtnLimpar_Click();

            DplPeriodo.Items.Clear();
            DplPeriodo.Add("30", "Mensalmente");
            DplPeriodo.Add("15", "A cada 15 dias");
            DplPeriodo.Add("7", "Semanalmente");
            DplPeriodo.Add("0", "Personalizado");

            StateHasChanged();

        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            DtpEmissao.Value = DateTime.Today;
            DtpVencimento.Value = DateTime.Today;
            TxtParcelas.Value = 1;
            TxtParcelas_KeyUp();

            await TabSet.Active("Principal");

            TxtDocumento.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            if (TxtDocumento.Text.ToStringOrNull() == null)
            {
                throw new EmptyException("Informe o número documento!", TxtDocumento.Element);
            }

            if (ViewPesquisaPessoa.Value.ToIntOrNull() == null)
            {
                throw new EmptyException("Informe o fornecedor!", ViewPesquisaPessoa.Element);
            }

            if (DtpEmissao.Value == null)
            {
                throw new EmptyException("Informe a data de emissão!", DtpEmissao.Element);
            }

            if (ViewPesquisaPlanoConta.Value.ToIntOrNull() == null)
            {
                throw new EmptyException("Informe o plano de contas!", ViewPesquisaPlanoConta.Element);
            }

            if (ViewPesquisaContaBancaria.Value.ToIntOrNull() == null)
            {
                throw new EmptyException("Informe a conta bancária!", ViewPesquisaContaBancaria.Element);
            }

            if (ViewPesquisaFormaPagamento.Value.ToIntOrNull() == null)
            {
                throw new EmptyException("Informe a forma de pagamento!", ViewPesquisaFormaPagamento.Element);
            }

            if (TxtValor.Value == 0)
            {
                throw new EmptyException("Informe o valor!", TxtValor.Element);
            }

            if (DtpVencimento.Value == null)
            {
                throw new EmptyException("Informe a data de vencimento!", DtpVencimento.Element);
            }

            if (TxtParcelas.Value == 0)
            {
                throw new EmptyException("Informe a quantidade de parcelas!", TxtParcelas.Element);
            }


            var Titulo = new Titulo();

            foreach (var item in ListTituloDetalhe)
            {

                item.PessoaID = ViewPesquisaPessoa.Value.ToIntOrNull();
                item.Pessoa = null;

                item.PlanoContaID = ViewPesquisaPlanoConta.Value.ToIntOrNull();
                item.PlanoConta = null;

                item.ContaBancariaID = ViewPesquisaContaBancaria.Value.ToIntOrNull();
                item.ContaBancaria = null;

                item.FormaPagamentoID = ViewPesquisaFormaPagamento.Value.ToIntOrNull();
                item.FormaPagamento = null;

                item.CentroCustoID = ViewPesquisaCentroCusto.Value.ToIntOrNull();
                item.CentroCusto = null;

                Titulo.TituloDetalhe.Add(item);

            }

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(Titulo);

            await HelpUpdate.SaveChanges();

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void TxtDocumento_Input()
        {
            CarregarGrid();
        }

        protected void DtpEmissao_Change()
        {
            CarregarGrid();
        }

        protected void TxtValor_KeyUp()
        {
            CarregarGrid();
        }

        protected void DtpVencimento_Change()
        {
            CarregarGrid();
        }

        protected void TxtParcelas_KeyUp()
        {
            DplPeriodo.SelectedValue = "30";
            CarregarGrid();
        }

        protected void DplPeriodo_Change()
        {
            TxtPeriodo.Focus();
            CarregarGrid();
        }

        protected void TxtPeriodo_KeyUp()
        {
            CarregarGrid();
        }

        private void CarregarGrid()
        {

            ListTituloDetalhe.Clear();

            DateTime? DataVencimento = DateTime.Today;

            for (var i = 1; i <= Convert.ToInt32(TxtParcelas.Value); i++)
            {

                if (i == 1)
                {
                    DataVencimento = DtpVencimento.Value;
                }
                else
                {
                    switch (DplPeriodo.SelectedValue)
                    {
                        case "0":
                            DataVencimento = DataVencimento?.AddDays((int)TxtPeriodo.Value);
                            break;
                        case "30":
                            DataVencimento = DataVencimento?.AddMonths(1);
                            break;
                        default:
                            DataVencimento = DataVencimento?.AddDays(DplPeriodo.SelectedValue.ToInt());
                            break;
                    }
                }

                var vTotal = Math.Round(TxtValor.Value / TxtParcelas.Value, 2);

                if (i == TxtParcelas.Value)
                {
                    vTotal = Math.Round(TxtValor.Value - ListTituloDetalhe.Sum(c => c.vTotal ?? 0), 2);
                }

                ListTituloDetalhe.Add(new TituloDetalhe() { DataEmissao = DtpEmissao.Value, nDocumento = TxtDocumento.Text, nParcela = i, vTotal = vTotal, vLiquido = vTotal, DataVencimento = DataVencimento?.ToUniversalTime() });

            }

            GridViewTituloDetalhe.Refresh();

        }

        protected void ViewPesquisaFormaPagamento_BeforePesquisar()
        {
            if (ViewPesquisaContaBancaria.Value.ToIntOrNull() == null)
            {
                ViewPesquisaFormaPagamento.Clear();
                ViewPesquisaContaBancaria.Focus();
                throw new Exception("Informe a conta bancária!");
            }
        }

        protected void ViewPesquisaContaBancaria_Change(object args)
        {
            ContaBancaria_Change(args, ViewPesquisaFormaPagamento, ViewPesquisaContaBancaria);
        }

        protected void ViewPesquisaFormaPagamento_Change(object args)
        {
            FormaPagamento_Change(args, ViewPesquisaContaBancaria, ViewPesquisaFormaPagamento);
        }


        private void ContaBancaria_Change(object args, ViewPesquisa<FormaPagamento> ViewPesquisaFormaPagamento, ViewPesquisa<ContaBancaria> ViewPesquisaContaBancaria)
        {
            var Predicate = "ContaBancariaFormaPagamento.Any(ContaBancariaID == @0)";

            if (args == null)
            {
                ViewPesquisaFormaPagamento.Where.Remove(ViewPesquisaFormaPagamento.Where.FirstOrDefault(c => c.Predicate == Predicate));
                ViewPesquisaFormaPagamento.AddWhere(Predicate, 0);
                ViewPesquisaFormaPagamento.Clear();
            }
            else
            {
                ViewPesquisaFormaPagamento.Where.Remove(ViewPesquisaFormaPagamento.Where.FirstOrDefault(c => c.Predicate == Predicate));
                ViewPesquisaFormaPagamento.AddWhere(Predicate, ViewPesquisaContaBancaria.Value.ToIntOrNull());
            }
        }

        private void FormaPagamento_Change(object args, ViewPesquisa<ContaBancaria> ViewPesquisaContaBancaria, ViewPesquisa<FormaPagamento> ViewPesquisaFormaPagamento)
        {
            if (args != null)
            {
                if (ViewPesquisaContaBancaria.Value.ToIntOrNull() == null)
                {
                    App.JSRuntime.InvokeVoidAsync("alert", "Informe a conta bancária!");
                    ViewPesquisaFormaPagamento.Clear();
                    ViewPesquisaContaBancaria.Focus();
                }
            }
        }


        protected async Task GridViewTituloDetalheDtpVencimento_Change(object args, TituloDetalhe TituloDetalhe)
        {
            try
            {
                TituloDetalhe.DataVencimento = Convert.ToDateTime(((ChangeEventArgs)args).Value.ToString());
            }
            catch (Exception ex)
            {
                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

    }
}