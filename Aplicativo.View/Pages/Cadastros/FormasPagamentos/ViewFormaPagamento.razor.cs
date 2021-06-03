using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.FormasPagamentos
{
    public partial class ViewFormaPagamentoPage : ComponentBase
    {

        public FormaPagamento ViewModel = new FormaPagamento();

        [Parameter] public ListItemViewLayout<FormaPagamento> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }

        #endregion

        protected async Task Page_Load(object args)
        {

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<FormaPagamento>();

            Query.AddWhere("FormaPagamentoID == @0", ((FormaPagamento)args).FormaPagamentoID);

            var ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            TxtCodigo.Text = ViewModel.FormaPagamentoID.ToStringOrNull();
            TxtDescricao.Text = ViewModel.Descricao.ToStringOrNull();

        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            await TabSet.Active("Principal");

            TxtDescricao.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            if (string.IsNullOrEmpty(TxtDescricao.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe a descrição!", TxtDescricao.Element);
            }

            ViewModel = new FormaPagamento();

            ViewModel.FormaPagamentoID = TxtCodigo.Text.ToIntOrNull();
            ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

            var Query = new HelpQuery<FormaPagamento>();

            ViewModel = await Query.Update(ViewModel);

            await App.JSRuntime.InvokeVoidAsync("alert", "Salvo com sucesso!!");

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

            var Query = new HelpQuery<FormaPagamento>();

            Query.AddWhere("FormaPagamentoID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            await Query.Update(ViewModel, false);

        }

    }
}