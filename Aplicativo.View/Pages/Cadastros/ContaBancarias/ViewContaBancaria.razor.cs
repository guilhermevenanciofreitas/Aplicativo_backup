using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.ContaBancarias
{
    public partial class ViewContaBancariaPage<TValue> : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        public EditItemViewLayout<ContaBancaria> EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }
        

        public ViewContaBancariaFormaPagamento ViewContaBancariaFormaPagamento { get; set; }
        #endregion

        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView = new List<ContaBancariaFormaPagamento>();
            ViewContaBancariaFormaPagamento.ListItemViewLayout.Refresh();

            await TabSet.Active("Principal");

            TxtDescricao.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddInclude("ContaBancariaFormaPagamento");
            Query.AddInclude("ContaBancariaFormaPagamento.ContaBancaria");
            Query.AddInclude("ContaBancariaFormaPagamento.FormaPagamento");
            Query.AddWhere("ContaBancariaID == @0", ((ContaBancaria)args)?.ContaBancariaID);
            Query.AddTake(1);

            EditItemViewLayout.ViewModel = await EditItemViewLayout.Carregar<ContaBancaria>(Query);


            TxtCodigo.Text = EditItemViewLayout.ViewModel.ContaBancariaID.ToStringOrNull();
            TxtDescricao.Text = EditItemViewLayout.ViewModel.Descricao.ToStringOrNull();

            ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView = EditItemViewLayout.ViewModel.ContaBancariaFormaPagamento.ToList();
            ViewContaBancariaFormaPagamento.ListItemViewLayout.Refresh();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtDescricao.Text))
            {
                await TabSet.Active("Principal");
                await HelpEmptyException.New(JSRuntime, TxtDescricao.Element, "Informe a descrição!");
            }

            EditItemViewLayout.ViewModel.ContaBancariaFormaPagamento = ViewContaBancariaFormaPagamento.ListItemViewLayout.ListItemView;

            EditItemViewLayout.ViewModel = await EditItemViewLayout.Update(EditItemViewLayout.ViewModel);

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
                await EditItemViewLayout.Carregar(EditItemViewLayout.ViewModel);
            else
                EditItemViewLayout.ViewModal.Hide();
          
            
        }

        protected async Task ViewLayout_Excluir()
        {
            await EditItemViewLayout.Delete(EditItemViewLayout.ViewModel);
        }
    }
}