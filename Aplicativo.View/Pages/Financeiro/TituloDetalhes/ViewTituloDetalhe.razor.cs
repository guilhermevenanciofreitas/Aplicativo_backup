using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Financeiro.TituloDetalhes
{
    public partial class ViewTituluDetalhePage : ComponentBase
    {

        private decimal pJuros = 0;
        private decimal pMulta = 0;

        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtNumeroDocumento { get; set; }
        public DatePicker DtpEmissao { get; set; }
        public DatePicker DtpVencimento { get; set; }


        public NumericBox TxtTotal { get; set; }
        public NumericBox TxtPercDesconto { get; set; }
        public NumericBox TxtValorDesconto { get; set; }
        public NumericBox TxtPercJuros { get; set; }
        public NumericBox TxtValorJuros { get; set; }
        public NumericBox TxtDiasAtrasados { get; set; }
        public NumericBox TxtPercMulta { get; set; }
        public NumericBox TxtValorMulta { get; set; }

        public decimal vLiquido { get; set; }

        public TextArea TxtObservacao { get; set; }

        #endregion

        protected async Task ViewLayout_Load(object args)
        {

            await ViewLayout_Limpar();

            if (args == null) return;

            var Query = new HelpQuery<TituloDetalhe>();

            Query.AddInclude("ContaBancaria");
            Query.AddInclude("ContaBancaria.ContaBancariaFormaPagamento");
            Query.AddWhere("TituloDetalheID == @0", ((TituloDetalhe)args).TituloDetalheID);

            var ViewModel = await Query.FirstOrDefault();

            var ContaBancariaFormaPagamento = ViewModel.ContaBancaria.ContaBancariaFormaPagamento.FirstOrDefault(c => c.ContaBancariaID == ViewModel.ContaBancariaID && c.FormaPagamentoID == ViewModel.FormaPagamentoID);

            pJuros = ContaBancariaFormaPagamento?.pJuros ?? 0;
            pMulta = ContaBancariaFormaPagamento?.pMulta ?? 0;

            TxtCodigo.Text = ViewModel.TituloDetalheID.ToStringOrNull();
            TxtNumeroDocumento.Text = ViewModel.nDocumento.ToStringOrNull();
            DtpEmissao.Value = ViewModel.DataEmissao;
            DtpVencimento.Value = ViewModel.DataVencimento;



            TxtTotal.Value = ViewModel.vTotal ?? 0;

            TxtPercDesconto.Value = ViewModel.pDesconto ?? 0;
            TxtValorDesconto.Value = ViewModel.vDesconto ?? 0;

            TxtPercJuros.Value = ViewModel.pJuros ?? 0;
            TxtValorJuros.Value = ViewModel.vJuros ?? 0;

            TxtDiasAtrasados.Value = Convert.ToDecimal(ViewModel.DiasAtrasados ?? 0);

            TxtPercMulta.Value = ViewModel.pMulta ?? 0;
            TxtValorMulta.Value = ViewModel.vMulta ?? 0;

            vLiquido = ViewModel.vLiquido ?? 0;

            TxtObservacao.Text = ViewModel.Observacao.ToStringOrNull();

            await DtpVencimento_Leave();

        }

        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            await TabSet.Active("Principal");

            TxtNumeroDocumento.Focus();

            vLiquido = 0;

        }

        protected async Task ViewLayout_Salvar()
        {

            var ViewModel = new TituloDetalhe();

            ViewModel.TituloDetalheID = TxtCodigo.Text.ToIntOrNull();

            ViewModel.nDocumento = TxtNumeroDocumento.Text.ToStringOrNull();
            ViewModel.DataEmissao = DtpEmissao.Value;
            ViewModel.DataVencimento = DtpVencimento.Value;

            ViewModel.vTotal = TxtTotal.Value;

            ViewModel.pDesconto = Math.Round(TxtPercDesconto.Value, 3);
            ViewModel.vDesconto = Math.Round(TxtValorDesconto.Value, 2);

            ViewModel.pJuros = Math.Round(TxtPercJuros.Value, 3);
            ViewModel.vJuros = Math.Round(TxtValorJuros.Value, 2);

            ViewModel.DiasAtrasados = Convert.ToInt32(TxtDiasAtrasados.Value);

            ViewModel.pMulta = Math.Round(TxtPercMulta.Value, 3);
            ViewModel.vMulta = Math.Round(TxtValorMulta.Value, 2);

            ViewModel.vLiquido = Math.Round(vLiquido, 2);

            ViewModel.Observacao = TxtObservacao.Text.ToStringOrNull();


            var Query = new HelpQuery<TituloDetalhe>();

            ViewModel = await Query.Update(ViewModel);


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                TxtCodigo.Text = ViewModel.TituloDetalheID.ToStringOrNull();
            }
            else
            {
                EditItemViewLayout.ViewModal.Hide();
            }

        }

        protected async Task ViewLayout_Excluir()
        {

            await Excluir(new List<int> { TxtCodigo.Text.ToInt() });

            EditItemViewLayout.ViewModal.Hide();

        }

        public async Task Excluir(List<int> args)
        {

            var Query = new HelpQuery<TituloDetalhe>();

            Query.AddWhere("TituloDetalheID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            //foreach (var item in ViewModel)
            //{
            //    item.Ativo = false;
            //}

            await Query.Update(ViewModel, false);

        }

        protected async Task DtpVencimento_Leave()
        {

            if (pJuros == 0 && pMulta == 0)
            {
                return;
            }

            if (DtpVencimento.Value < DateTime.Today)
            {
                var DiasAtrasados = (DateTime.Today - DtpVencimento.Value).Value.TotalDays;

                if (DiasAtrasados != Convert.ToDouble(TxtDiasAtrasados.Value))
                {

                    var r = await App.JSRuntime.InvokeAsync<bool>("confirm", "Documento vai ficar com data de vencimento atrasado, com isso e aplicado juros e multa!\n\nDeseja atualizar os valores?");

                    if (r)
                    {

                        TxtPercJuros.Value = pJuros;
                        TxtPercJuros_KeyUp();

                        TxtDiasAtrasados.Value = (decimal)((DateTime.Today - DtpVencimento.Value)?.TotalDays);

                        TxtPercMulta.Value = pMulta;
                        TxtPercMulta_KeyUp();

                    }
                }
            }
            else
            {
                TxtPercJuros.Value = 0;
                TxtValorJuros.Value = 0;

                TxtDiasAtrasados.Value = 0;

                TxtPercMulta.Value = 0;
                TxtValorMulta.Value = 0;

            }

        }

        protected void TxtTotal_KeyUp()
        {

            TxtValorDesconto_KeyUp();

            TxtPercJuros_KeyUp();
            TxtPercMulta_KeyUp();

            CalcularValorLiquido();

        }

        protected void TxtPercDesconto_KeyUp()
        {
            TxtValorDesconto.Value = (TxtTotal.Value / 100) * TxtPercDesconto.Value;
            CalcularValorLiquido();
        }

        protected void TxtValorDesconto_KeyUp()
        {
            if (TxtTotal.Value != 0)
                TxtPercDesconto.Value = (TxtValorDesconto.Value / TxtTotal.Value) * 100;
            else
                TxtPercDesconto.Value = 0;

            CalcularValorLiquido();
        }

        protected void TxtPercJuros_KeyUp()
        {
            TxtValorJuros.Value = (TxtTotal.Value / 100) * TxtPercJuros.Value;
            CalcularValorLiquido();
        }

        protected void TxtValorJuros_KeyUp()
        {
            if (TxtTotal.Value != 0)
                TxtPercJuros.Value = (TxtValorJuros.Value / TxtTotal.Value) * 100;
            else
                TxtPercJuros.Value = 0;

            CalcularValorLiquido();
        }

        protected void TxtDiasAtrasados_KeyUp()
        {
            CalcularValorLiquido();
        }

        protected void TxtPercMulta_KeyUp()
        {
            TxtValorMulta.Value = (TxtTotal.Value / 100) * TxtPercMulta.Value;
            CalcularValorLiquido();
        }

        protected void TxtValorMulta_KeyUp()
        {
            if (TxtTotal.Value != 0)
                TxtPercMulta.Value = (TxtValorMulta.Value / TxtTotal.Value) * 100;
            else
                TxtPercMulta.Value = 0;

            CalcularValorLiquido();

        }

        private void CalcularValorLiquido()
        {

            var vDesconto = Math.Round(TxtValorDesconto.Value, 2);
            var vJuros = Math.Round(TxtValorJuros.Value, 2);
            var vMulta = Math.Round(TxtValorMulta.Value, 2);

            var DiasAtrasados = TxtDiasAtrasados.Value;

            vLiquido = Math.Round((TxtTotal.Value - vDesconto) + (vJuros * DiasAtrasados) + vMulta, 2);

        }


        //[Parameter]
        //public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        //public EditItemViewLayout<TituloDetalhe> EditItemViewLayout { get; set; }


        //private decimal pJuros = 0;
        //private decimal pMulta = 0;

        //#region Elements

        //public TabSet TabSet { get; set; }

        //public TextBox TxtCodigo { get; set; }
        //public TextBox TxtNumeroDocumento { get; set; }
        //public DatePicker DtpEmissao { get; set; }
        //public DatePicker DtpVencimento { get; set; }


        //public NumericBox TxtTotal { get; set; }
        //public NumericBox TxtPercDesconto { get; set; }
        //public NumericBox TxtValorDesconto { get; set; }
        //public NumericBox TxtPercJuros { get; set; }
        //public NumericBox TxtValorJuros { get; set; }
        //public NumericBox TxtDiasAtrasados { get; set; }
        //public NumericBox TxtPercMulta { get; set; }
        //public NumericBox TxtValorMulta { get; set; }

        //public decimal ValorLiquido { get; set; }

        //public TextArea TxtObservacao { get; set; }

        //#endregion


        //protected void ViewLayout_PageLoad()
        //{
        //    EditItemViewLayout.BtnLimpar.Visible = false;
        //}

        //protected async Task ViewLayout_Limpar()
        //{

        //    EditItemViewLayout.LimparCampos(this);

        //    await TabSet.Active("Principal");

        //    TxtNumeroDocumento.Focus();

        //}

        //protected async Task ViewLayout_Carregar(object args)
        //{

        //    var Query = new HelpQuery<TituloDetalhe>();

        //    Query.AddInclude("ContaBancaria");
        //    Query.AddInclude("ContaBancaria.ContaBancariaFormaPagamento");
        //    Query.AddWhere("TituloDetalheID == @0", ((TituloDetalhe)args)?.TituloDetalheID);
        //    Query.AddTake(1);

        //    EditItemViewLayout.ViewModel = await Query.FirstOrDefault();







        //    var ContaBancariaFormaPagamento = EditItemViewLayout.ViewModel.ContaBancaria.ContaBancariaFormaPagamento.FirstOrDefault(c => c.ContaBancariaID == EditItemViewLayout.ViewModel.ContaBancariaID && c.FormaPagamentoID == EditItemViewLayout.ViewModel.FormaPagamentoID);

        //    pJuros = ContaBancariaFormaPagamento?.pJuros ?? 0;
        //    pMulta = ContaBancariaFormaPagamento?.pMulta ?? 0;

        //    TxtCodigo.Text = EditItemViewLayout.ViewModel.TituloDetalheID.ToStringOrNull();
        //    TxtNumeroDocumento.Text = EditItemViewLayout.ViewModel.nDocumento.ToStringOrNull();
        //    DtpEmissao.Value = EditItemViewLayout.ViewModel.DataEmissao;
        //    DtpVencimento.Value = EditItemViewLayout.ViewModel.DataVencimento;



        //    TxtTotal.Value = EditItemViewLayout.ViewModel.vTotal ?? 0;

        //    TxtPercDesconto.Value = EditItemViewLayout.ViewModel.pDesconto ?? 0;
        //    TxtValorDesconto.Value = EditItemViewLayout.ViewModel.vDesconto ?? 0;

        //    TxtPercJuros.Value = EditItemViewLayout.ViewModel.pJuros ?? 0;
        //    TxtValorJuros.Value = EditItemViewLayout.ViewModel.vJuros ?? 0;

        //    TxtDiasAtrasados.Value = Convert.ToDecimal(EditItemViewLayout.ViewModel.DiasAtrasados ?? 0);

        //    TxtPercMulta.Value = EditItemViewLayout.ViewModel.pMulta ?? 0;
        //    TxtValorMulta.Value = EditItemViewLayout.ViewModel.vMulta ?? 0;

        //    ValorLiquido = EditItemViewLayout.ViewModel.vLiquido ?? 0;

        //    TxtObservacao.Text = EditItemViewLayout.ViewModel.Observacao.ToStringOrNull();

        //    await DtpVencimento_Leave();

        //}

        //protected async Task ViewLayout_Salvar()
        //{

        //    EditItemViewLayout.ViewModel.TituloDetalheID = TxtCodigo.Text.ToIntOrNull();

        //    EditItemViewLayout.ViewModel.nDocumento = TxtNumeroDocumento.Text.ToStringOrNull();
        //    EditItemViewLayout.ViewModel.DataEmissao = DtpEmissao.Value;
        //    EditItemViewLayout.ViewModel.DataVencimento = DtpVencimento.Value;

        //    EditItemViewLayout.ViewModel.vTotal = TxtTotal.Value;

        //    EditItemViewLayout.ViewModel.pDesconto = Math.Round(TxtPercDesconto.Value, 3);
        //    EditItemViewLayout.ViewModel.vDesconto = Math.Round(TxtValorDesconto.Value, 2);

        //    EditItemViewLayout.ViewModel.pJuros = Math.Round(TxtPercJuros.Value, 3);
        //    EditItemViewLayout.ViewModel.vJuros = Math.Round(TxtValorJuros.Value, 2);

        //    EditItemViewLayout.ViewModel.DiasAtrasados = Convert.ToInt32(TxtDiasAtrasados.Value);

        //    EditItemViewLayout.ViewModel.pMulta = Math.Round(TxtPercMulta.Value, 3);
        //    EditItemViewLayout.ViewModel.vMulta = Math.Round(TxtValorMulta.Value, 2);

        //    EditItemViewLayout.ViewModel.vLiquido = Math.Round(ValorLiquido, 2);

        //    EditItemViewLayout.ViewModel.Observacao = TxtObservacao.Text.ToStringOrNull();

        //    EditItemViewLayout.ViewModel = await EditItemViewLayout.Update(EditItemViewLayout.ViewModel);

        //    if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
        //        await EditItemViewLayout.Carregar(EditItemViewLayout.ViewModel);
        //    else
        //        EditItemViewLayout.ViewModal.Hide();

        //}

        //protected async Task ViewLayout_Excluir()
        //{
        //    await EditItemViewLayout.Delete(EditItemViewLayout.ViewModel);
        //}


        //protected async Task DtpVencimento_Leave()
        //{

        //    if (pJuros == 0 && pMulta == 0)
        //    {
        //        return;
        //    }

        //    if (DtpVencimento.Value < DateTime.Today)
        //    {
        //        var DiasAtrasados = (DateTime.Today - DtpVencimento.Value).Value.TotalDays;

        //        if (DiasAtrasados != Convert.ToDouble(TxtDiasAtrasados.Value))
        //        {

        //            var r = await App.JSRuntime.InvokeAsync<bool>("confirm", "Documento vai ficar com data de vencimento atrasado, com isso e aplicado juros e multa!\n\nDeseja atualizar os valores?");

        //            if (r)
        //            {

        //                TxtPercJuros.Value = pJuros;
        //                TxtPercJuros_KeyUp();

        //                TxtDiasAtrasados.Value = (decimal)((DateTime.Today - DtpVencimento.Value)?.TotalDays);

        //                TxtPercMulta.Value = pMulta;
        //                TxtPercMulta_KeyUp();

        //            }
        //        }
        //    }
        //    else
        //    {
        //        TxtPercJuros.Value = 0;
        //        TxtValorJuros.Value = 0;

        //        TxtDiasAtrasados.Value = 0;

        //        TxtPercMulta.Value = 0;
        //        TxtValorMulta.Value = 0;

        //    }

        //}

        //protected void TxtTotal_KeyUp()
        //{

        //    TxtValorDesconto_KeyUp();

        //    TxtPercJuros_KeyUp();
        //    TxtPercMulta_KeyUp();

        //    CalcularValorLiquido();

        //}

        //protected void TxtPercDesconto_KeyUp()
        //{
        //    TxtValorDesconto.Value = (TxtTotal.Value / 100) * TxtPercDesconto.Value;
        //    CalcularValorLiquido();
        //}

        //protected void TxtValorDesconto_KeyUp()
        //{
        //    if (TxtTotal.Value != 0)
        //        TxtPercDesconto.Value = (TxtValorDesconto.Value / TxtTotal.Value) * 100;
        //    else
        //        TxtPercDesconto.Value = 0;

        //    CalcularValorLiquido();
        //}

        //protected void TxtPercJuros_KeyUp()
        //{
        //    TxtValorJuros.Value = (TxtTotal.Value / 100) * TxtPercJuros.Value;
        //    CalcularValorLiquido();
        //}

        //protected void TxtValorJuros_KeyUp()
        //{
        //    if (TxtTotal.Value != 0)
        //        TxtPercJuros.Value = (TxtValorJuros.Value / TxtTotal.Value) * 100;
        //    else
        //        TxtPercJuros.Value = 0;

        //    CalcularValorLiquido();
        //}

        //protected void TxtDiasAtrasados_KeyUp()
        //{
        //    CalcularValorLiquido();
        //}

        //protected void TxtPercMulta_KeyUp()
        //{
        //    TxtValorMulta.Value = (TxtTotal.Value / 100) * TxtPercMulta.Value;
        //    CalcularValorLiquido();
        //}

        //protected void TxtValorMulta_KeyUp()
        //{
        //    if (TxtTotal.Value != 0)
        //        TxtPercMulta.Value = (TxtValorMulta.Value / TxtTotal.Value) * 100;
        //    else
        //        TxtPercMulta.Value = 0;

        //    CalcularValorLiquido();

        //}

        //private void CalcularValorLiquido()
        //{

        //    var vDesconto = Math.Round(TxtValorDesconto.Value, 2);
        //    var vJuros = Math.Round(TxtValorJuros.Value, 2);
        //    var vMulta = Math.Round(TxtValorMulta.Value, 2);

        //    var DiasAtrasados = TxtDiasAtrasados.Value;

        //    ValorLiquido = Math.Round((TxtTotal.Value - vDesconto) + (vJuros * DiasAtrasados) + vMulta, 2);

        //}


    }
}