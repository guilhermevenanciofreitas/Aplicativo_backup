//using Aplicativo.Utils.Helpers;
//using Aplicativo.Utils.Models;
//using Aplicativo.View.Controls;
//using Aplicativo.View.Helpers;
//using Aplicativo.View.Layout;
//using Microsoft.AspNetCore.Components;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Aplicativo.View.Pages.Estoque.Requisicoes
//{
//    public class ViewRequisicaoItemPage : ComponentBase
//    {

//        public ListItemViewLayout<RequisicaoItem> ListItemViewLayout { get; set; }
//        public EditItemViewLayout<RequisicaoItem> EditItemViewLayout { get; set; }


//        public TextBox TxtQuantidade { get; set; }
//        public TextBox TxtCodigoBarras { get; set; }


//        protected void ViewLayout_PageLoad()
//        {

//            EditItemViewLayout.BtnSalvar.Label = "Confirmar";
//            EditItemViewLayout.BtnSalvar.Width = "140px";

//        }
 
//        protected void ViewLayout_Limpar()
//        {

//            EditItemViewLayout.LimparCampos(this);

//            TxtQuantidade.Text = "1";

//        }

//        protected async Task ViewLayout_ItemView(object args)
//        {
//            await EditItemViewLayout.Carregar((RequisicaoItem)args);
//        }

//        protected void ViewLayout_Carregar(object args)
//        {

//            EditItemViewLayout.ViewModel = (RequisicaoItem)args;

//            TxtCodigoBarras.Text = EditItemViewLayout.ViewModel.EstoqueMovimentoItemEntrada.CodigoBarra.ToStringOrNull();

//        }

//        protected async Task ViewLayout_Salvar()
//        {

//            EditItemViewLayout.ViewModel.Quantidade = TxtQuantidade.Text.ToDecimalOrNull();

//            var Query = new HelpQuery<EstoqueMovimentoItem>();

//            Query.AddInclude("Produto");
//            Query.AddInclude("EstoqueMovimentoItemEntrada");
//            Query.AddInclude("EstoqueMovimentoItemSaida");
//            Query.AddWhere("EstoqueMovimentoItemEntrada.CodigoBarra == @0", TxtCodigoBarras.Text);
//            Query.AddTake(1);

//            var EstoqueMovimentoItem = await Query.FirstOrDefault();

//            if (EstoqueMovimentoItem == null)
//            {
//                //await HelpEmptyException.New(JSRuntime, TxtCodigoBarras.Element, "Código de barras não encontrado!");
//            }

//            EditItemViewLayout.ViewModel.ProdutoID = EstoqueMovimentoItem.ProdutoID;
//            EditItemViewLayout.ViewModel.Produto = EstoqueMovimentoItem.Produto;
//            EditItemViewLayout.ViewModel.EstoqueMovimentoItemEntradaID = EstoqueMovimentoItem.EstoqueMovimentoItemEntradaID;

//            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
//            {
//                ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
//            }

//            ListItemViewLayout.GridViewItem?.Refresh();
//            //ListItemViewLayout.Refresh();

//            EditItemViewLayout.ViewModal.Hide();

//        }

//        protected void ViewLayout_Delete(object args)
//        {
//            foreach(var item in (List<RequisicaoItem>)args) ListItemViewLayout.ListItemView.Remove(item);
//            EditItemViewLayout.ViewModal.Hide();
//        }

//    }
//}