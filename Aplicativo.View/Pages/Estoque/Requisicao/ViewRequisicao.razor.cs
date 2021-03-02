using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Material.Icon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Estoque.Requisicao
{
    public class ViewRequisicaoPage : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout<Utils.Models.Requisicao> ListItemViewLayout { get; set; }
        public EditItemViewLayout<Utils.Models.Requisicao> EditItemViewLayout { get; set; }


        protected TextBox TxtCodigo { get; set; }
        protected TextBox TxtDescricao { get; set; }

        protected DateTimePicker DtpSaida { get; set; }
        protected DateTimePicker DtpEntrada { get; set; }

        protected TextArea TxtObservacao { get; set; }


        protected ViewRequisicaoItem ViewRequisicaoItem { get; set; }

        private Utils.Models.Requisicao Requisicao = new Utils.Models.Requisicao();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

                EditItemViewLayout.ItemViewButtons.Add(new ItemViewButton() { Icon = new FilterListIcon(), Label = "Finalizar", OnClick = BtnFinalizar_Click });

            }
        }

        protected void ViewLayout_Limpar()
        {

            Requisicao = new Utils.Models.Requisicao();


            EditItemViewLayout.BtnSalvar.Disabled = false;
            EditItemViewLayout.BtnExcluir.Disabled = false;
            EditItemViewLayout.ItemViewButtons[0].Disabled = false;

            //Principal
            TxtCodigo.Text = null;
            TxtDescricao.Text = null;

            DtpSaida.Value = DateTime.Now;
            DtpEntrada.Value = null;

            TxtObservacao.Text = null;


            ViewRequisicaoItem.ListItemViewLayout.ListItemView = new List<RequisicaoItem>();
            ViewRequisicaoItem.ListItemViewLayout.Refresh();


            TxtDescricao.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Request = new Request();

            Request.Parameters.Add(new Parameters("RequisicaoID", ((Utils.Models.Requisicao)args)?.RequisicaoID));

            Requisicao = await HelpHttp.Send<Utils.Models.Requisicao>(Http, "api/Requisicao/Get", Request);


            EditItemViewLayout.BtnSalvar.Disabled = (Requisicao.DataEntrada != null);
            EditItemViewLayout.BtnExcluir.Disabled = (Requisicao.DataEntrada != null);
            EditItemViewLayout.ItemViewButtons[0].Disabled = (Requisicao.DataEntrada != null);
            

            //Principal
            TxtCodigo.Text = Requisicao.RequisicaoID.ToStringOrNull();
            //TxtDescricao.Text = Requisicao.Descricao.ToStringOrNull();

            DtpSaida.Value = Requisicao.DataSaida;
            DtpEntrada.Value = Requisicao.DataEntrada;

            TxtObservacao.Text = Requisicao.Observacao.ToStringOrNull();


            ViewRequisicaoItem.ListItemViewLayout.ListItemView = Requisicao.RequisicaoItem.ToList();
            ViewRequisicaoItem.ListItemViewLayout.Refresh();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (DtpSaida.Value == null)
            {
                throw new EmptyException("Informe a data da saída!", DtpSaida.Element);
            }

            //Principal
            Requisicao.RequisicaoID = TxtCodigo.Text.ToIntOrNull();

            Requisicao.DataSaida = DtpSaida.Value;
            Requisicao.Observacao = TxtObservacao.Text.ToStringOrNull();

            Requisicao.RequisicaoItem = ViewRequisicaoItem.ListItemViewLayout.ListItemView;


            var Request = new Request();

            var Requisicoes = new List<Utils.Models.Requisicao> { Requisicao };

            Request.Parameters.Add(new Parameters("Requisicoes", Requisicoes));

            Requisicoes = await HelpHttp.Send<List<Utils.Models.Requisicao>>(Http, "api/Requisicao/Save", Request);

            Requisicao = Requisicoes.FirstOrDefault();

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                await EditItemViewLayout.Carregar(Requisicao);
            }
            else
            {
                EditItemViewLayout.ViewModal.Hide();
            }
            
        }

        protected async void BtnFinalizar_Click()
        {
            try
            {

                if (DtpEntrada.Value == null)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Informe a data da entrada!");
                    DtpEntrada.Focus();
                    return;
                }

                await HelpLoading.Show(this, "Finalizado...");

                EditItemViewLayout.ItemViewMode = ItemViewMode.New;

                await ViewLayout_Salvar();

                //Editar dados
                Requisicao.DataEntrada = DtpEntrada.Value;


                var Request = new Request();

                var Requisicoes = new List<Utils.Models.Requisicao> { Requisicao };

                Request.Parameters.Add(new Parameters("Requisicoes", Requisicoes));

                Requisicoes = await HelpHttp.Send<List<Utils.Models.Requisicao>>(Http, "api/Requisicao/Finalizar", Request);

                Requisicao = Requisicoes.FirstOrDefault();

                EditItemViewLayout.ViewModal.Hide();

                await ListItemViewLayout.ShowToast("", "Requisição finalizada com sucesso!");

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        protected async Task ViewLayout_Excluir()
        {

            var Request = new Request();

            var Requisicoes = new List<int?> { Requisicao.RequisicaoID };

            Request.Parameters.Add(new Parameters("Requisicoes", Requisicoes));

            await HelpHttp.Send(Http, "api/Requisicao/Delete", Request);

        }
    }
}