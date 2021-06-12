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

        public decimal vTotal_Items { get; set; } = 0;
        public decimal vTotal_Pagamento { get; set; } = 0;

        public TituloDetalhe ViewModel { get; set; } = new TituloDetalhe();

        public ListItemViewLayout<TituloDetalhe> ListView { get; set; }
        public EditItemViewLayout NewItemViewLayout { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        protected bool Informacoes { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public NumericBox New_TxtValor { get; set; }
        public DatePicker New_DtpVencimento { get; set; }
        public NumericBox New_TxtParcelas { get; set; }
        public DropDownList New_DplPeriodo { get; set; }
        public NumericBox New_TxtPeriodo { get; set; }

        public ViewPesquisa<PlanoConta> New_ViewPesquisaPlanoConta { get; set; }
        public ViewPesquisa<CentroCusto> New_ViewPesquisaCentroCusto { get; set; }
        public ViewPesquisa<ContaBancaria> New_ViewPesquisaContaBancaria { get; set; }
        public ViewPesquisa<FormaPagamento> New_ViewPesquisaFormaPagamento { get; set; }
       

        public ViewPesquisa<PlanoConta> Edit_ViewPesquisaPlanoConta { get; set; }
        public ViewPesquisa<CentroCusto> Edit_ViewPesquisaCentroCusto { get; set; }
        public ViewPesquisa<ContaBancaria> Edit_ViewPesquisaContaBancaria { get; set; }
        public ViewPesquisa<FormaPagamento> Edit_ViewPesquisaFormaPagamento { get; set; }
        public NumericBox Edit_TxtValor { get; set; }
        public DatePicker Edit_DtpVencimento { get; set; }



        public SfGrid<TituloDetalhe> GridViewTituloDetalhe { get; set; }
        public List<TituloDetalhe> ListTituloDetalhe { get; set; } = new List<TituloDetalhe>();
        #endregion

        #region ListView
        protected void Page_Load()
        {

            New_ViewPesquisaPlanoConta.Value = "1";
            New_ViewPesquisaPlanoConta.Text = "Receita 1";

            New_ViewPesquisaPlanoConta.AddWhere("Ativo == @0", true);
            New_ViewPesquisaPlanoConta.AddWhere("PlanoContaTipoID == @0", 1);

            New_ViewPesquisaCentroCusto.AddWhere("Ativo == @0", true);

            New_ViewPesquisaContaBancaria.AddWhere("Ativo == @0", true);
            New_ViewPesquisaFormaPagamento.AddWhere("Ativo == @0", true);




            Edit_ViewPesquisaPlanoConta.AddWhere("Ativo == @0", true);
            Edit_ViewPesquisaPlanoConta.AddWhere("PlanoContaTipoID == @0", 1);

            Edit_ViewPesquisaCentroCusto.AddWhere("Ativo == @0", true);

            Edit_ViewPesquisaContaBancaria.AddWhere("Ativo == @0", true);
            Edit_ViewPesquisaFormaPagamento.AddWhere("Ativo == @0", true);

        }

        protected async Task ViewLayout_ItemView(object args)
        {

            Informacoes = false;

            if (args == null)
            {
                NewItemViewLayout.ItemViewMode = ItemViewMode.New;
                await New(args);
                
            }
            else
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                await Edit(args);
            }
        }
        #endregion

        private void InitializeComponents()
        {

            New_DplPeriodo.Items.Clear();
            New_DplPeriodo.Add("30", "Mensalmente");
            New_DplPeriodo.Add("15", "A cada 15 dias");
            New_DplPeriodo.Add("7", "Semanalmente");
            New_DplPeriodo.Add("0", "Personalizado");

        }

        #region ViewPage
        protected void ViewLayout_NewLimpar()
        {

            InitializeComponents();

            NewItemViewLayout.LimparCampos(this);

            New_ViewPesquisaContaBancaria.Clear();
            New_ViewPesquisaFormaPagamento.Clear();


            var vRestante = vTotal_Items - vTotal_Pagamento;

            if (vRestante >= 0)
            {
                New_TxtValor.Value = vRestante;
            }
            
            New_DtpVencimento.Value = DateTime.Today;
            New_TxtParcelas.Value = 1;
            TxtParcelas_KeyUp();

            New_ViewPesquisaContaBancaria.Focus();

        }

        protected void ViewLayout_EditLimpar()
        {

            InitializeComponents();

            EditItemViewLayout.LimparCampos(this);

            Edit_ViewPesquisaContaBancaria.Clear();
            Edit_ViewPesquisaFormaPagamento.Clear();

            Edit_TxtValor.Value = 0;
            Edit_DtpVencimento.Value = null;

            Edit_ViewPesquisaContaBancaria.Focus();

        }

        protected async Task New(object args)
        {

            await NewItemViewLayout.Show(args);

            ViewLayout_NewLimpar();

        }

        protected async Task Edit(object args)
        {

            await EditItemViewLayout.Show(args);

            ViewLayout_EditLimpar();

            if (args == null) return;

            ViewModel = (TituloDetalhe)args;


            Edit_ViewPesquisaPlanoConta.Value = ViewModel.PlanoContaID.ToStringOrNull();
            Edit_ViewPesquisaPlanoConta.Text = ViewModel.PlanoConta?.Descricao.ToStringOrNull();

            Edit_ViewPesquisaCentroCusto.Value = ViewModel.CentroCustoID.ToStringOrNull();
            Edit_ViewPesquisaCentroCusto.Text = ViewModel.CentroCusto?.Descricao.ToStringOrNull();

            Edit_ViewPesquisaContaBancaria.Value = ViewModel.ContaBancariaID.ToStringOrNull();
            Edit_ViewPesquisaContaBancaria.Text = ViewModel.ContaBancaria?.Descricao.ToStringOrNull();

            Edit_ViewPesquisaFormaPagamento.Value = ViewModel.FormaPagamentoID.ToStringOrNull();
            Edit_ViewPesquisaFormaPagamento.Text = ViewModel.FormaPagamento?.Descricao.ToStringOrNull();

            Edit_TxtValor.Value = ViewModel.vLiquido ?? 0;
            Edit_DtpVencimento.Value = ViewModel.DataVencimento;

            ViewPesquisaEdit_ContaBancaria_Change(ViewModel.ContaBancaria);


        }

        protected async Task ViewPageBtnNewSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtNome.Text))
            //{
            //    throw new EmptyException("Informe o nome!", TxtNome.Element);
            //}

            foreach (var item in ListTituloDetalhe)
            {

                //item.PessoaID = ViewPesquisaPessoa.Value.ToIntOrNull();

                item.PlanoContaID = New_ViewPesquisaPlanoConta.Value.ToIntOrNull();
                item.PlanoConta = new PlanoConta() { Descricao = New_ViewPesquisaPlanoConta.Text };

                item.CentroCustoID = New_ViewPesquisaCentroCusto.Value.ToIntOrNull();
                item.CentroCusto = new CentroCusto() { Descricao = New_ViewPesquisaCentroCusto.Text };

                item.ContaBancariaID = New_ViewPesquisaContaBancaria.Value.ToIntOrNull();
                item.ContaBancaria = new ContaBancaria() { Descricao = New_ViewPesquisaContaBancaria.Text };

                item.FormaPagamentoID = New_ViewPesquisaFormaPagamento.Value.ToIntOrNull();
                item.FormaPagamento = new FormaPagamento() { Descricao = New_ViewPesquisaFormaPagamento.Text };

            }

            if (NewItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.AddRange(ListTituloDetalhe);
            }

            var nParcela = 1;
            foreach (var item in ListView.Items)
            {
                item.nParcela = nParcela;
                nParcela++;
            }

            await NewItemViewLayout.ViewModal.Hide();

            CalculaPagamento();

        }

        protected async Task ViewPageBtnEditSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtNome.Text))
            //{
            //    throw new EmptyException("Informe o nome!", TxtNome.Element);
            //}


            ViewModel.PlanoContaID = Edit_ViewPesquisaPlanoConta.Value.ToIntOrNull();
            ViewModel.PlanoConta = new PlanoConta() { Descricao = Edit_ViewPesquisaPlanoConta.Text };

            ViewModel.CentroCustoID = Edit_ViewPesquisaCentroCusto.Value.ToIntOrNull();
            ViewModel.CentroCusto = new CentroCusto() { Descricao = Edit_ViewPesquisaCentroCusto.Text };

            ViewModel.ContaBancariaID = Edit_ViewPesquisaContaBancaria.Value.ToIntOrNull();
            ViewModel.ContaBancaria = new ContaBancaria() { Descricao = Edit_ViewPesquisaContaBancaria.Text };

            ViewModel.FormaPagamentoID = Edit_ViewPesquisaFormaPagamento.Value.ToIntOrNull();
            ViewModel.FormaPagamento = new FormaPagamento() { Descricao = Edit_ViewPesquisaFormaPagamento.Text };


            ViewModel.vTotal = Edit_TxtValor.Value;
            ViewModel.vLiquido = ViewModel.vTotal;

            ViewModel.DataVencimento = Edit_DtpVencimento.Value;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            var nParcela = 1;
            foreach (var item in ListView.Items)
            {
                item.nParcela = nParcela;
                nParcela++;
            }

            await EditItemViewLayout.ViewModal.Hide();

            CalculaPagamento();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<TituloDetalhe>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

            CalculaPagamento();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<TituloDetalhe>().ToList());

            CalculaPagamento();

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

        protected void BtnMaisInformacoes_Click()
        {
            Informacoes = true;
            StateHasChanged();
        }

        protected void BtnMenosInformacoes_Click()
        {
            Informacoes = false;
            StateHasChanged();
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
            New_DplPeriodo.SelectedValue = "30";
            CarregarGrid();
        }

        protected void DplPeriodo_Change()
        {
            New_TxtPeriodo.Focus();
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

            for (var i = 1; i <= Convert.ToInt32(New_TxtParcelas.Value); i++)
            {

                if (i == 1)
                {
                    DataVencimento = New_DtpVencimento.Value;
                }
                else
                {
                    switch (New_DplPeriodo.SelectedValue)
                    {
                        case "0":
                            DataVencimento = DataVencimento?.AddDays((int)New_TxtPeriodo.Value);
                            break;
                        case "30":
                            DataVencimento = DataVencimento?.AddMonths(1);
                            break;
                        default:
                            DataVencimento = DataVencimento?.AddDays(New_DplPeriodo.SelectedValue.ToInt());
                            break;
                    }
                }

                var vTotal = Math.Round(New_TxtValor.Value / New_TxtParcelas.Value, 2);

                if (i == New_TxtParcelas.Value)
                {
                    vTotal = Math.Round(New_TxtValor.Value - ListTituloDetalhe.Sum(c => c.vTotal ?? 0), 2);
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

        public void CalculaPagamento()
        {
            vTotal_Pagamento = ListView.Items.Sum(c => c.vLiquido ?? 0);
            StateHasChanged();
        }


        protected void ViewPesquisaNew_FormaPagamento_BeforePesquisar()
        {
            if (New_ViewPesquisaContaBancaria.Value.ToIntOrNull() == null)
            {
                New_ViewPesquisaFormaPagamento.Clear();
                New_ViewPesquisaContaBancaria.Focus();
                throw new Exception("Informe a conta bancária!");
            }
        }

        protected void ViewPesquisaNew_ContaBancaria_Change(object args)
        {
            ContaBancaria_Change(args, New_ViewPesquisaFormaPagamento, New_ViewPesquisaContaBancaria);
        }

        protected void ViewPesquisaNew_FormaPagamento_Change(object args)
        {
            FormaPagamento_Change(args, New_ViewPesquisaContaBancaria, New_ViewPesquisaFormaPagamento);
        }

        protected void ViewPesquisaEdit_FormaPagamento_BeforePesquisar()
        {
            if (Edit_ViewPesquisaContaBancaria.Value.ToIntOrNull() == null)
            {
                Edit_ViewPesquisaFormaPagamento.Clear();
                Edit_ViewPesquisaContaBancaria.Focus();
                throw new Exception("Informe a conta bancária!");
            }
        }

        protected void ViewPesquisaEdit_ContaBancaria_Change(object args)
        {
            ContaBancaria_Change(args, Edit_ViewPesquisaFormaPagamento, Edit_ViewPesquisaContaBancaria);
        }

        protected void ViewPesquisaEdit_FormaPagamento_Change(object args)
        {
            FormaPagamento_Change(args, Edit_ViewPesquisaContaBancaria, Edit_ViewPesquisaFormaPagamento);
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
                    ViewPesquisaFormaPagamento.Focus();
                }
            }
        }

        #endregion
    
    }
}