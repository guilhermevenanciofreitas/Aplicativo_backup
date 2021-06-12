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
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.ContaBancarias
{
    public partial class ViewContaBancariaPage : ComponentBase
    {

        [Parameter] public ListItemViewLayout<ContaBancaria> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }


        public ViewContaBancariaFormaPagamento ViewContaBancariaFormaPagamento { get; set; }
        #endregion

        protected async Task Page_Load(object args)
        {

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<ContaBancaria>();

            Query.AddInclude("ContaBancariaFormaPagamento");
            Query.AddInclude("ContaBancariaFormaPagamento.ContaBancaria");
            Query.AddInclude("ContaBancariaFormaPagamento.FormaPagamento");
            Query.AddWhere("ContaBancariaID == @0", ((ContaBancaria)args).ContaBancariaID);

            var ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            TxtCodigo.Text = ViewModel.ContaBancariaID.ToStringOrNull();
            TxtDescricao.Text = ViewModel.Descricao.ToStringOrNull();

            ViewContaBancariaFormaPagamento.ListView.Items = ViewModel.ContaBancariaFormaPagamento.ToList();

        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            ViewContaBancariaFormaPagamento.ListView.Items = new List<ContaBancariaFormaPagamento>();

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


            var ViewModel = new ContaBancaria();

            ViewModel.ContaBancariaID = TxtCodigo.Text.ToIntOrNull();
            ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

            ViewModel.ContaBancariaFormaPagamento = ViewContaBancariaFormaPagamento.ListView.Items.ToList();

            foreach(var item in ViewModel.ContaBancariaFormaPagamento)
            {
                item.FormaPagamento = null;
            }


            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            var Changes = await HelpUpdate.SaveChanges();

            ViewModel = HelpUpdate.Bind<ContaBancaria>(Changes[0]);


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

            var Query = new HelpQuery<ContaBancaria>();

            Query.AddWhere("ContaBancariaID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            //await Query.Update(ViewModel, false);

        }

    }
}