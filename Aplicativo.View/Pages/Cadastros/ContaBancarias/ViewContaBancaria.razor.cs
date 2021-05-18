using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.ContaBancarias
{
    public partial class ViewContaBancariaPage : ComponentBase
    {

        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }


        public ViewContaBancariaFormaPagamento ViewContaBancariaFormaPagamento { get; set; }
        #endregion

        protected async Task ViewLayout_Load(object args)
        {

            await ViewLayout_Limpar();

            if (args == null) return;

            var Query = new HelpQuery<ContaBancaria>();

            Query.AddInclude("ContaBancariaFormaPagamento");
            Query.AddInclude("ContaBancariaFormaPagamento.ContaBancaria");
            Query.AddInclude("ContaBancariaFormaPagamento.FormaPagamento");
            Query.AddWhere("ContaBancariaID == @0", ((ContaBancaria)args).ContaBancariaID);

            var ViewModel = await Query.FirstOrDefault();

            TxtCodigo.Text = ViewModel.ContaBancariaID.ToStringOrNull();
            TxtDescricao.Text = ViewModel.Descricao.ToStringOrNull();

            ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView = ViewModel.ContaBancariaFormaPagamento.Cast<object>().ToList();

        }

        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView = new List<object>();

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


            var ViewModel = new ContaBancaria();

            ViewModel.ContaBancariaID = TxtCodigo.Text.ToIntOrNull();
            ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

            ViewModel.ContaBancariaFormaPagamento = ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView.Cast<ContaBancariaFormaPagamento>().ToList(); ;


            var Query = new HelpQuery<ContaBancaria>();

            ViewModel = await Query.Update(ViewModel);


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                TxtCodigo.Text = ViewModel.ContaBancariaID.ToStringOrNull();
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

            var Query = new HelpQuery<ContaBancaria>();

            Query.AddWhere("ContaBancariaID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            await Query.Update(ViewModel, false);

        }

        //[Parameter]
        //public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        //public EditItemViewLayout<ContaBancaria> EditItemViewLayout { get; set; }

        //#region Elements

        //public TabSet TabSet { get; set; }

        //public TextBox TxtCodigo { get; set; }
        //public TextBox TxtDescricao { get; set; }


        //public ViewContaBancariaFormaPagamento ViewContaBancariaFormaPagamento { get; set; }
        //#endregion

        //protected void ViewLayout_PageLoad()
        //{

        //}

        //protected async Task ViewLayout_Limpar()
        //{

        //    EditItemViewLayout.LimparCampos(this);

        //    ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView = new List<ContaBancariaFormaPagamento>();
        //    //ViewContaBancariaFormaPagamento.ListItemViewLayout.Refresh();

        //    await TabSet.Active("Principal");

        //    TxtDescricao.Focus();

        //}

        //protected async Task ViewLayout_Carregar(object args)
        //{

        //    var Query = new HelpQuery<ContaBancaria>();

        //    Query.AddInclude("ContaBancariaFormaPagamento");
        //    Query.AddInclude("ContaBancariaFormaPagamento.ContaBancaria");
        //    Query.AddInclude("ContaBancariaFormaPagamento.FormaPagamento");
        //    Query.AddWhere("ContaBancariaID == @0", ((ContaBancaria)args)?.ContaBancariaID);

        //    EditItemViewLayout.ViewModel = await Query.FirstOrDefault();


        //    TxtCodigo.Text = EditItemViewLayout.ViewModel.ContaBancariaID.ToStringOrNull();
        //    TxtDescricao.Text = EditItemViewLayout.ViewModel.Descricao.ToStringOrNull();

        //    ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView = EditItemViewLayout.ViewModel.ContaBancariaFormaPagamento.ToList();
        //    //ViewContaBancariaFormaPagamento.ListItemViewLayout.Refresh();

        //}

        //protected async Task ViewLayout_Salvar()
        //{

        //    if (string.IsNullOrEmpty(TxtDescricao.Text))
        //    {
        //        await TabSet.Active("Principal");
        //        throw new EmptyException("Informe a descrição!", TxtDescricao.Element);
        //    }

        //    EditItemViewLayout.ViewModel.ContaBancariaID = TxtCodigo.Text.ToIntOrNull();
        //    EditItemViewLayout.ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

        //    EditItemViewLayout.ViewModel.ContaBancariaFormaPagamento = ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView;

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