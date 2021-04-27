using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.FormaPagamentos
{
    public partial class ViewFormaPagamentoPage<TValue> : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        public EditItemViewLayout<FormaPagamento> EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }
        
        #endregion

        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            await TabSet.Active("Principal");

            TxtDescricao.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddWhere("FormaPagamentoID == @0", ((FormaPagamento)args)?.FormaPagamentoID);
            Query.AddTake(1);

            EditItemViewLayout.ViewModel = await EditItemViewLayout.Carregar<FormaPagamento>(Query);

            TxtCodigo.Text = EditItemViewLayout.ViewModel.FormaPagamentoID.ToStringOrNull();
            TxtDescricao.Text = EditItemViewLayout.ViewModel.Descricao.ToStringOrNull();
            
        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtDescricao.Text))
            {
                await TabSet.Active("Principal");
                await HelpEmptyException.New(JSRuntime, TxtDescricao.Element, "Informe a descrição!");
            }

            EditItemViewLayout.ViewModel.FormaPagamentoID = TxtCodigo.Text.ToIntOrNull();
            EditItemViewLayout.ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

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