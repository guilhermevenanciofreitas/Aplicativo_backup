//using Aplicativo.Utils;
//using Aplicativo.Utils.Helpers;
//using Aplicativo.Utils.Models;
//using Aplicativo.View.Controls;
//using Aplicativo.View.Helpers;
//using Aplicativo.View.Helpers.Exceptions;
//using Aplicativo.View.Layout;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Aplicativo.View.Pages.Estoque.Requisicoes
//{
//    public partial class ViewRequisicaoPage<TValue> : ComponentBase
//    {

//        protected ViewModal ViewModalFinalizar { get; set; }
//        public DateTimePicker DtpFinalizarEntrada { get; set; }

//        [Parameter]
//        public ListItemViewLayout<Requisicao> ListItemViewLayout { get; set; }
//        public EditItemViewLayout<Requisicao> EditItemViewLayout { get; set; }

//        #region Elements

        

//        public TextBox TxtCodigo { get; set; }
//        public TextBox TxtDescricao { get; set; }
//        public TextArea TxtObservacao { get; set; }
//        public DateTimePicker DtpSaida { get; set; }
//        public DateTimePicker DtpEntrada { get; set; }

//        protected TabSet TabSet { get; set; }

//        protected ViewRequisicaoItem ViewRequisicaoItem { get; set; }
//        #endregion

//        protected void ViewLayout_PageLoad()
//        {

//            EditItemViewLayout.BtnExcluir.Visible = false;

//            EditItemViewLayout.ItemViewButtons.Add(new ItemViewButton() { Label = "Finalizar", OnClick = BtnFinalizar_Click });
//        }

//        protected async Task ViewLayout_Limpar()
//        {

//            EditItemViewLayout.BtnSalvar.Disabled = false;
//            EditItemViewLayout.Disable(this, false);

//            TxtCodigo.ReadOnly = true;

//            EditItemViewLayout.LimparCampos(this);

//            ViewRequisicaoItem.ListItemViewLayout.ListItemView = new List<RequisicaoItem>();
//            //ViewRequisicaoItem.ListItemViewLayout.Refresh();

//            TxtDescricao.Focus();

//            await TabSet.Active("Principal");

//        }

//        protected async Task ViewLayout_Carregar(object args)
//        {

//            var Query = new HelpQuery<Requisicao>();

//            Query.AddInclude("RequisicaoItem");
//            Query.AddInclude("RequisicaoItem.Produto");
//            Query.AddInclude("RequisicaoItem.EstoqueMovimentoItemEntrada");
//            Query.AddWhere("RequisicaoID == @0", ((Requisicao)args)?.RequisicaoID);

//            EditItemViewLayout.ViewModel = await Query.FirstOrDefault();


//            EditItemViewLayout.BtnSalvar.Disabled = EditItemViewLayout.ViewModel.DataEntrada != null;
//            EditItemViewLayout.ItemViewButtons[0].Disabled = EditItemViewLayout.ViewModel.DataEntrada != null;

//            EditItemViewLayout.Disable(this, EditItemViewLayout.ViewModel.DataEntrada != null);
//            TxtCodigo.ReadOnly = true;

//            TxtCodigo.Text = EditItemViewLayout.ViewModel.RequisicaoID.ToStringOrNull();

//            DtpSaida.Value = EditItemViewLayout.ViewModel.DataSaida;
//            DtpEntrada.Value = EditItemViewLayout.ViewModel.DataEntrada;
//            TxtObservacao.Text = EditItemViewLayout.ViewModel.Observacao.ToStringOrNull();


//            ViewRequisicaoItem.ListItemViewLayout.ListItemView = EditItemViewLayout.ViewModel.RequisicaoItem.ToList();
//            //ViewRequisicaoItem.ListItemViewLayout.Refresh();

//        }

//        protected async Task ViewLayout_Salvar()
//        {

//            if (DtpSaida.Value == null)
//            {
//                await TabSet.Active("Principal");
//                throw new EmptyException("Informe a data da saída!", DtpSaida.Element);
//            }

//            if (DtpEntrada.Value != null)
//            {
//                await TabSet.Active("Principal");
//                throw new EmptyException("Informe a data da entrada somente ao finalizar!", DtpEntrada.Element);
//                //await HelpEmptyException.New(JSRuntime, DtpEntrada.Element, "Informe a data da entrada somente ao finalizar!");
//            }

//            if (ViewRequisicaoItem.ListItemViewLayout.ListItemView.Count == 0)
//            {
//                await TabSet.Active("Itens");
//                throw new EmptyException("Nenhum item inserido!");
//                //await HelpEmptyException.New(JSRuntime, "Nenhum item inserido!");
//            }

//            EditItemViewLayout.ViewModel.RequisicaoID = TxtCodigo.Text.ToIntOrNull();
//            EditItemViewLayout.ViewModel.DataSaida = DtpSaida.Value;
//            EditItemViewLayout.ViewModel.DataEntrada = DtpEntrada.Value;
//            EditItemViewLayout.ViewModel.Observacao = TxtObservacao.Text.ToStringOrNull();

//            EditItemViewLayout.ViewModel.RequisicaoItem = ViewRequisicaoItem.ListItemViewLayout.ListItemView;

//            foreach(var item in EditItemViewLayout.ViewModel.RequisicaoItem)
//            {
//                item.Produto = null;
//            }

//            var Request = new Request();

//            Request.Parameters.Add(new Parameters(typeof(Requisicao).Name, new List<object> { EditItemViewLayout.ViewModel }));

//            EditItemViewLayout.ViewModel = (await HelpHttp.Send<List<Requisicao>>("api/Requisicao/Salvar", Request)).FirstOrDefault();

//            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
//                await EditItemViewLayout.Carregar(EditItemViewLayout.ViewModel);
//            else
//                EditItemViewLayout.ViewModal.Hide();

//        }

//        protected async void ViewModalFinalizar_Validate()
//        {

//            if (DtpFinalizarEntrada.Value == null)
//            {
//                await App.JSRuntime.InvokeVoidAsync("alert", "Informe a data da entrada!");
//                DtpFinalizarEntrada.Focus();
//                return;
//            }

//            ViewModalFinalizar.Confirm();

//        }

//        protected async void BtnFinalizar_Click()
//        {

//            var List = EditItemViewLayout.SelectedValue();

//            try
//            {

//                foreach (var item in List)
//                {
//                    if (item.DataEntrada != null)
//                    {
//                        await App.JSRuntime.InvokeVoidAsync("alert", "Requisição Nº " + item.RequisicaoID + " já está finalizada!");
//                        return;
//                    }
//                }

//                DateTime? DataEntrada = null;

//                //Verifica se está finalizando pelo modal
//                if (!EditItemViewLayout.ViewModal.Open)
//                {
//                    var Confirm = await ViewModalFinalizar.ShowAsync();
//                    if (!Confirm) return;

//                    DataEntrada = DtpFinalizarEntrada.Value;
//                }
//                else
//                {
//                    if (DtpEntrada.Value == null)
//                    {
//                        await App.JSRuntime.InvokeVoidAsync("alert", "Informe a data da entrada!");
//                        DtpEntrada.Focus();
//                        return;
//                    }

//                    DataEntrada = DtpEntrada.Value;

//                }



//                foreach (var item in List)
//                {
//                    item.DataEntrada = DataEntrada;
//                }

//                var Request = new Request();

//                Request.Parameters.Add(new Parameters("Requisicao", List));

//                List = await HelpHttp.Send<List<Requisicao>>("api/Requisicao/Finalizar", Request);

//                EditItemViewLayout.ViewModel = List.FirstOrDefault();

//                EditItemViewLayout.ViewModal.Hide();

//                await ListItemViewLayout.ShowToast("Informação:", "Requisição finalizada com sucesso!", "e-toast-success", "e-success toast-icons");

//                await ListItemViewLayout.BtnPesquisar_Click();


//            }
//            catch (Exception ex)
//            {

//                foreach(var item in List)
//                {
//                    item.DataEntrada = null;
//                }

//                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
//            }
//        }

//        protected async Task ViewLayout_Excluir()
//        {
//            await EditItemViewLayout.Delete(EditItemViewLayout.ViewModel);
//        }
//    }
//}