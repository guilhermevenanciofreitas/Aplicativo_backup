using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.FormasPagamentos
{
    public partial class ViewFormaPagamentoPage : ComponentBase
    {

        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }

        #endregion

        protected async Task ViewLayout_Load(object args)
        {

            await ViewLayout_Limpar();

            if (args == null) return;

            var Query = new HelpQuery<FormaPagamento>();

            Query.AddWhere("FormaPagamentoID == @0", ((FormaPagamento)args).FormaPagamentoID);

            var ViewModel = await Query.FirstOrDefault();

            TxtCodigo.Text = ViewModel.FormaPagamentoID.ToStringOrNull();
            TxtDescricao.Text = ViewModel.Descricao.ToStringOrNull();

        }

        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            await TabSet.Active("Principal");

            TxtDescricao.Focus();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtDescricao.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe a descrição!", TxtDescricao.Element);
            }

            var ViewModel = new FormaPagamento();

            ViewModel.FormaPagamentoID = TxtCodigo.Text.ToIntOrNull();
            ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

            var Query = new HelpQuery<FormaPagamento>();

            ViewModel = await Query.Update(ViewModel);


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                TxtCodigo.Text = ViewModel.FormaPagamentoID.ToStringOrNull();
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

            var Query = new HelpQuery<FormaPagamento>();

            Query.AddWhere("FormaPagamentoID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            await Query.Update(ViewModel, false);

        }

        //[Parameter]
        //public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        //public EditItemViewLayout<FormaPagamento> EditItemViewLayout { get; set; }

        //#region Elements

        //public TabSet TabSet { get; set; }

        //public TextBox TxtCodigo { get; set; }
        //public TextBox TxtDescricao { get; set; }

        //#endregion

        //protected void ViewLayout_PageLoad()
        //{

        //}

        //protected async Task ViewLayout_Limpar()
        //{

        //    EditItemViewLayout.LimparCampos(this);

        //    await TabSet.Active("Principal");

        //    TxtDescricao.Focus();

        //}

        //protected async Task ViewLayout_Carregar(object args)
        //{

        //    var Query = new HelpQuery<FormaPagamento>();

        //    Query.AddWhere("FormaPagamentoID == @0", ((FormaPagamento)args)?.FormaPagamentoID);
        //    Query.AddTake(1);

        //    EditItemViewLayout.ViewModel = await Query.FirstOrDefault();

        //    TxtCodigo.Text = EditItemViewLayout.ViewModel.FormaPagamentoID.ToStringOrNull();
        //    TxtDescricao.Text = EditItemViewLayout.ViewModel.Descricao.ToStringOrNull();

        //}

        //protected async Task ViewLayout_Salvar()
        //{

        //    if (string.IsNullOrEmpty(TxtDescricao.Text))
        //    {
        //        await TabSet.Active("Principal");
        //        //await HelpEmptyException.New(JSRuntime, TxtDescricao.Element, "Informe a descrição!");
        //    }

        //    EditItemViewLayout.ViewModel.FormaPagamentoID = TxtCodigo.Text.ToIntOrNull();
        //    EditItemViewLayout.ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

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
    }
}