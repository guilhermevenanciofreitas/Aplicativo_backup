using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Aplicativo.View.Pages.Comercial.Faturamento;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Vendas
{
    public partial class ViewAndamentoPage : ComponentBase
    {

        public bool IsFinalizado { get; set; } = false;
        public bool IsConferido { get; set; } = false;
        public bool IsFaturado { get; set; } = false;


        public List<int?> PedidoVendaID { get; set; } = new List<int?>();
        //public int? PedidoVendaStatusID { get; set; }

        [Parameter] public ListItemViewLayout<PedidoVenda> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        [Parameter] public EventCallback OnConfirm { get; set; }
        [Parameter] public EventCallback OnFinally { get; set; }

        #region Elements
        public DropDownList DplStatus { get; set; }
        public TextArea TxtObservacao { get; set; }
        #endregion

        private async Task InitializeComponents()
        {

            var HelpQuery = new HelpQuery<PedidoVenda>();

            HelpQuery.AddInclude("PedidoVendaStatus");
            HelpQuery.AddInclude("PedidoVendaStatus.PedidoVendaAndamentoWorkflow");
            HelpQuery.AddInclude("PedidoVendaStatus.PedidoVendaAndamentoWorkflow.PedidoVendaStatusPara");

            HelpQuery.AddWhere("PedidoVendaID == @0", PedidoVendaID.FirstOrDefault());

            var PedidoVenda = await HelpQuery.FirstOrDefault();

            var PedidoVendaStatus = PedidoVenda.PedidoVendaStatus.PedidoVendaAndamentoWorkflow.Select(c => c.PedidoVendaStatusPara); //HelpParametros.Parametros.PedidoVendaStatus;


            //await App.JSRuntime.InvokeVoidAsync("console.log", PedidoVendaStatus);

            if (IsFinalizado)
            {
                PedidoVendaStatus = PedidoVendaStatus.Where(c => (c.IsFinalizado ?? false) == true).ToList();
            }

            if (IsConferido)
            {
                PedidoVendaStatus = PedidoVendaStatus.Where(c => (c.IsConferido ?? false) == true).ToList();
            }

            if (IsFaturado)
            {
                PedidoVendaStatus = PedidoVendaStatus.Where(c => (c.IsFaturado ?? false) == true).ToList();
            }

            DplStatus.LoadDropDownList("PedidoVendaStatusID", "Descricao", new DropDownListItem(null, "[Selecione]"), PedidoVendaStatus.ToList());

        }

        protected async Task Page_Load(object args)
        {

            await InitializeComponents();

            BtnLimpar_Click();

            //DplStatus.SelectedValue = PedidoVendaStatusID.ToStringOrNull();

            //if (args == null) return;

        }

        protected void BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            DplStatus.Focus();

        }

        protected async Task BtnConfirmar_Click()
        {

            try
            {

                if (DplStatus.SelectedValue.ToIntOrNull() == null)
                {
                    throw new EmptyException("Informe o status!", DplStatus.Element);
                }

                await OnConfirm.InvokeAsync(null);

                await Andamento(PedidoVendaID, DplStatus.SelectedValue.ToIntOrNull());

                await EditItemViewLayout.ViewModal.Hide();

                await OnFinally.InvokeAsync(null);

            }
            catch (EmptyException)
            {

            }
            catch (Exception ex)
            {
                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        public async Task Andamento(List<int?> PedidoVendaID, int? PedidoVendaStatusID)
        {


            var Status = HelpParametros.Parametros.PedidoVendaStatus.FirstOrDefault(c => c.PedidoVendaStatusID == PedidoVendaStatusID);


            if (Status.IsFaturado ?? false)
            {
                var Faturamento = new ViewFaturamentoPage();

                await Faturamento.Page_Load(PedidoVendaID);
                await Faturamento.ViewPedidoVendaAndamento_Confirm();

            }





            var Query = new HelpQuery<PedidoVenda>();

            Query.AddWhere("PedidoVendaID IN (" + string.Join(",", PedidoVendaID.ToArray()) + ")");

            var PedidoVenda = await Query.ToList();


            var Pedido = PedidoVenda?.FirstOrDefault();


            foreach(var item in PedidoVenda)
            {

                if (item.PedidoVendaStatusID == null)
                {
                    throw new Exception("Pedido número " + item.PedidoVendaID + " não possui status!");
                }

                if (item.PedidoVendaStatusID != Pedido.PedidoVendaStatusID)
                {
                    throw new Exception("Todos os pedidos devem ser o mesmo status!");
                }

            }



            var HelpUpdate = new HelpUpdate();

            foreach (var item in PedidoVenda)
            {


                if (Status.IsFinalizado ?? false)
                {
                    item.Finalizado = DateTime.Now;
                }

                if (Status.IsSeparado ?? false)
                {
                    item.Separado = DateTime.Now;
                }

                if (Status.IsConferido ?? false)
                {
                    item.Conferido = DateTime.Now;
                }

                //if (Status.IsFaturado ?? false)
                //{
                //    item.Faturado = DateTime.Now;
                //}

                var Andamento = new PedidoVendaAndamento();

                Andamento.PedidoVendaID = item.PedidoVendaID;
                Andamento.UsuarioID = HelpParametros.Parametros.UsuarioLogado.UsuarioID;
                Andamento.Data = DateTime.Now;
                Andamento.PedidoVendaStatusID = DplStatus.SelectedValue.ToIntOrNull();
                Andamento.Observacao = TxtObservacao.Text.ToStringOrNull();

                item.PedidoVendaStatusID = Andamento.PedidoVendaStatusID;

                HelpUpdate.Add(item);
                HelpUpdate.Add(Andamento);

            }


            await HelpUpdate.SaveChanges();


        }

    }
}