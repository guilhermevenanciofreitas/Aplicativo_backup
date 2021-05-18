using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
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

        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtDocumento { get; set; }
        public DatePicker DtpEmissao { get; set; }


        public NumericBox TxtValor { get; set; }
        public DatePicker DtpVencimento { get; set; }
        public NumericBox TxtParcelas { get; set; }
        public DropDownList DplPeriodo { get; set; }
        public NumericBox TxtPeriodo { get; set; }


        public SfGrid<TituloDetalhe> GridViewTituloDetalhe { get; set; }
        public List<TituloDetalhe> ListTituloDetalhe { get; set; } = new List<TituloDetalhe>();

        #endregion

        protected async Task ViewLayout_Load(object args)
        {

            await ViewLayout_Limpar();

            DplPeriodo.Items.Clear();
            DplPeriodo.Add("30", "Mensalmente");
            DplPeriodo.Add("15", "A cada 15 dias");
            DplPeriodo.Add("7", "Semanalmente");
            DplPeriodo.Add("0", "Personalizado");

        }

        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            DtpVencimento.Value = DateTime.Today;
            TxtParcelas.Value = 1;
            TxtParcelas_KeyUp();

            await TabSet.Active("Principal");

            TxtDocumento.Focus();

        }

        protected async Task ViewLayout_Salvar()
        {

            var Titulo = new Titulo();

            foreach (var item in ListTituloDetalhe)
            {
                Titulo.TituloDetalhe.Add(item);
            }

            var Query = new HelpQuery<Titulo>();

            await Query.Update(Titulo);

            EditItemViewLayout.ViewModal.Hide();

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

            DateTime DataVencimento = DateTime.Today;

            for (var i = 1; i <= Convert.ToInt32(TxtParcelas.Value); i++)
            {

                if (i == 1)
                {
                    DataVencimento = (DateTime)DtpVencimento.Value;
                }
                else
                {
                    switch (DplPeriodo.SelectedValue)
                    {
                        case "0":
                            DataVencimento = DataVencimento.AddDays((int)TxtPeriodo.Value);
                            break;
                        case "30":
                            DataVencimento = DataVencimento.AddMonths(1);
                            break;
                        default:
                            DataVencimento = DataVencimento.AddDays(DplPeriodo.SelectedValue.ToInt());
                            break;
                    }
                }

                var vTotal = Math.Round(TxtValor.Value / TxtParcelas.Value, 2);

                if (i == TxtParcelas.Value)
                {
                    vTotal = Math.Round(TxtValor.Value - ListTituloDetalhe.Sum(c => c.vTotal ?? 0), 2);
                }

                ListTituloDetalhe.Add(new TituloDetalhe() { DataEmissao = DtpEmissao.Value, nDocumento = TxtDocumento.Text, nParcela = i, vTotal = vTotal, vLiquido = vTotal, DataVencimento = DataVencimento.ToUniversalTime() });

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

    }
}