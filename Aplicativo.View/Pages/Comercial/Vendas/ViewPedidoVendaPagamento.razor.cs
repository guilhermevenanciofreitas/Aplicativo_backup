using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
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

namespace Aplicativo.View.Pages.Comercial.Vendas
{
    public class ViewPedidoVendaPagamentoPage : ComponentBase
    {

        public TituloDetalhe ViewModel { get; set; } = new TituloDetalhe();

        public ListItemViewLayout<TituloDetalhe> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        //public TextBox TxtDocumento { get; set; }
        //public DatePicker DtpEmissao { get; set; }

        //public ViewPesquisa ViewPesquisaPessoa { get; set; }

        public NumericBox TxtValor { get; set; }
        public DatePicker DtpVencimento { get; set; }
        public NumericBox TxtParcelas { get; set; }
        public DropDownList DplPeriodo { get; set; }
        public NumericBox TxtPeriodo { get; set; }

        //public ViewPesquisa ViewPesquisaPlanoConta { get; set; }
        public ViewPesquisa<ContaBancaria> ViewPesquisaContaBancaria { get; set; }
        public ViewPesquisa<FormaPagamento> ViewPesquisaFormaPagamento { get; set; }
        //public ViewPesquisa ViewPesquisaCentroCusto { get; set; }


        public SfGrid<TituloDetalhe> GridViewTituloDetalhe { get; set; }
        public List<TituloDetalhe> ListTituloDetalhe { get; set; } = new List<TituloDetalhe>();
        #endregion

        #region ListView
        protected void Page_Load()
        {
            ViewPesquisaContaBancaria.AddWhere("Ativo == @0", true);
            ViewPesquisaFormaPagamento.AddWhere("Ativo == @0", true);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        private void InitializeComponents()
        {

            DplPeriodo.Items.Clear();
            DplPeriodo.Add("30", "Mensalmente");
            DplPeriodo.Add("15", "A cada 15 dias");
            DplPeriodo.Add("7", "Semanalmente");
            DplPeriodo.Add("0", "Personalizado");

        }

        #region ViewPage
        protected void ViewLayout_Limpar()
        {

            InitializeComponents();

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaContaBancaria.Clear();
            ViewPesquisaFormaPagamento.Clear();

            DtpVencimento.Value = DateTime.Today;
            TxtParcelas.Value = 1;
            TxtParcelas_KeyUp();

            ViewPesquisaContaBancaria.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            ViewLayout_Limpar();

            if (args == null) return;

            ViewModel = (TituloDetalhe)args;

            ViewPesquisaContaBancaria.Value = ViewModel.ContaBancariaID.ToStringOrNull();
            ViewPesquisaContaBancaria.Text = ViewModel.ContaBancaria?.Descricao.ToStringOrNull();

            ViewPesquisaFormaPagamento.Value = ViewModel.FormaPagamentoID.ToStringOrNull();
            ViewPesquisaFormaPagamento.Text = ViewModel.FormaPagamento?.Descricao.ToStringOrNull();


            //TxtNome.Text = ViewModel.Contato.Nome.ToStringOrNull();
            //TxtTelefone.Text = ViewModel.Contato.Telefone.ToStringOrNull();
            //TxtEmail.Text = ViewModel.Contato.Email.ToStringOrNull();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtNome.Text))
            //{
            //    throw new EmptyException("Informe o nome!", TxtNome.Element);
            //}

            foreach (var item in ListTituloDetalhe)
            {

                //item.PessoaID = ViewPesquisaPessoa.Value.ToIntOrNull();
                //item.PlanoContaID = ViewPesquisaPlanoConta.Value.ToIntOrNull();
                item.ContaBancariaID = ViewPesquisaContaBancaria.Value.ToIntOrNull();
                item.ContaBancaria = new ContaBancaria() { Descricao = ViewPesquisaContaBancaria.Text };

                item.FormaPagamentoID = ViewPesquisaFormaPagamento.Value.ToIntOrNull();
                item.FormaPagamento = new FormaPagamento() { Descricao = ViewPesquisaFormaPagamento.Text };

                //item.CentroCustoID = ViewPesquisaCentroCusto.Value.ToIntOrNull();

            }

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.AddRange(ListTituloDetalhe);
            }

            var nParcela = 1;
            foreach (var item in ListView.Items)
            {
                item.nParcela = nParcela;
                nParcela++;
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<TituloDetalhe>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<TituloDetalhe>().ToList());

        }

        public void Excluir(List<TituloDetalhe> args)
        {

            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }

            var nParcela = 1;
            foreach (var item in ListView.Items)
            {
                item.nParcela = nParcela;
                nParcela++;
            }

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

                ListTituloDetalhe.Add(new TituloDetalhe() { nParcela = i, vTotal = vTotal, vLiquido = vTotal, DataVencimento = DataVencimento?.ToUniversalTime() });

            }

            GridViewTituloDetalhe.Refresh();

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

        protected void ViewPesquisaContaBancaria_Change(object args)
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

        protected void ViewPesquisaFormaPagamento_Change(object args)
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

        #endregion
    }
}