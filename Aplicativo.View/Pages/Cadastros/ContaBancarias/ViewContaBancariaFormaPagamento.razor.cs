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
        #endregion

        #region ListView
        protected void Page_Load()
        {
            ViewPesquisaFormaPagamento.AddWhere("Ativo == @0", true);
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


        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            ViewModel.FormaPagamentoID = ViewPesquisaFormaPagamento.Value.ToIntOrNull();
            ViewModel.FormaPagamento = new FormaPagamento() { Descricao = ViewPesquisaFormaPagamento.Text };
            
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

        #endregion

    }
}