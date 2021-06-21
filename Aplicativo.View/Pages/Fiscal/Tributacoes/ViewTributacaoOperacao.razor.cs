using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Fiscal.Tributacoes
{
    public class ViewTributacaoOperacaoPage : ComponentBase
    {

        public TributacaoOperacao ViewModel { get; set; } = new TributacaoOperacao();

        public ListItemViewLayout<TributacaoOperacao> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public ViewPesquisa<Operacao> ViewPesquisaOperacao { get; set; }

        public TabSet TabCFOP { get; set; }

        public ViewPesquisa<CFOP> ViewPesquisaCFOP_Estadual { get; set; }
        public ViewPesquisa<CFOP> ViewPesquisaCFOP_Interestadual { get; set; }
        public ViewPesquisa<CFOP> ViewPesquisaCFOP_Exterior { get; set; }

        #endregion

        #region ListView
       
        protected async Task ViewLayout_ItemView(object args)
        {

            ViewPesquisaCFOP_Estadual.Where.Clear();
            ViewPesquisaCFOP_Interestadual.Where.Clear();
            ViewPesquisaCFOP_Exterior.Where.Clear();

            ViewPesquisaCFOP_Estadual.AddWhere("Codigo.StartsWith(@0) || Codigo.StartsWith(@1)", "1.", "5.");
            ViewPesquisaCFOP_Interestadual.AddWhere("Codigo.StartsWith(@0) || Codigo.StartsWith(@1)", "2.", "6.");
            ViewPesquisaCFOP_Exterior.AddWhere("Codigo.StartsWith(@0) || Codigo.StartsWith(@1)", "3.", "7.");

            await ViewLayout_Carregar(args);

        }
        #endregion

        #region ViewPage
        protected async Task BtnLimpar_Click()
        {

            ViewModel = new TributacaoOperacao();

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaOperacao.Clear();

            ViewPesquisaCFOP_Estadual.Clear();
            ViewPesquisaCFOP_Interestadual.Clear();
            ViewPesquisaCFOP_Exterior.Clear();

            await TabCFOP.Active("Estadual");

            ViewPesquisaOperacao.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            await BtnLimpar_Click();

            if (args == null) return;

            ViewModel = (TributacaoOperacao)args;

            ViewPesquisaOperacao.Value = ViewModel.OperacaoID.ToStringOrNull();
            ViewPesquisaOperacao.Text = ViewModel.Operacao?.Descricao.ToStringOrNull();

            ViewPesquisaCFOP_Estadual.Value = ViewModel.Codigo_CFOP_Estadual.ToStringOrNull();
            ViewPesquisaCFOP_Estadual.Text = ViewModel.CFOP_Estadual?.Descricao.ToStringOrNull();

            ViewPesquisaCFOP_Interestadual.Value = ViewModel.Codigo_CFOP_Interestadual.ToStringOrNull();
            ViewPesquisaCFOP_Interestadual.Text = ViewModel.CFOP_Interestadual?.Descricao.ToStringOrNull();

            ViewPesquisaCFOP_Exterior.Value = ViewModel.Codigo_CFOP_Exterior.ToStringOrNull();
            ViewPesquisaCFOP_Exterior.Text = ViewModel.CFOP_Exterior?.Descricao.ToStringOrNull();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtSmtp.Text))
            //    throw new EmptyException("Informe o SMTP!", TxtSmtp.Element);

            ViewModel.OperacaoID = ViewPesquisaOperacao.Value.ToIntOrNull();
            ViewModel.Operacao = new Operacao() { Descricao = ViewPesquisaOperacao.Text };

            ViewModel.Codigo_CFOP_Estadual = ViewPesquisaCFOP_Estadual.Value.ToStringOrNull();
            ViewModel.CFOP_Estadual = new CFOP() { Descricao = ViewPesquisaCFOP_Estadual.Text };

            ViewModel.Codigo_CFOP_Interestadual = ViewPesquisaCFOP_Interestadual.Value.ToStringOrNull();
            ViewModel.CFOP_Interestadual = new CFOP() { Descricao = ViewPesquisaCFOP_Interestadual.Text };

            ViewModel.Codigo_CFOP_Exterior = ViewPesquisaCFOP_Exterior.Value.ToStringOrNull();
            ViewModel.CFOP_Exterior = new CFOP() { Descricao = ViewPesquisaCFOP_Exterior.Text };


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<TributacaoOperacao>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<TributacaoOperacao>().ToList());

        }

        public void Excluir(List<TributacaoOperacao> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        #endregion

    }
}