using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Estoque.Requisicao
{
    public class ViewRequisicaoItemPage : HelpComponent
    {

        public ListItemViewLayout<RequisicaoItem> ListItemViewLayout { get; set; }
        public EditItemViewLayout<RequisicaoItem> EditItemViewLayout { get; set; }


        protected TextBox TxtQuantidade { get; set; }

        protected TextBox TxtCodigoBarras { get; set; }

        protected RequisicaoItem RequisicaoItem { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

                EditItemViewLayout.BtnSalvar.Label = "Confirmar";
                EditItemViewLayout.BtnSalvar.Width = "140px";

            }
        }

        protected void ViewLayout_Limpar()
        {

            RequisicaoItem = new RequisicaoItem();

            TxtQuantidade.Text = "1";
            TxtCodigoBarras.Text = null;

            //TxtSmtp.Text = null;
            //TxtPorta.Text = null;
            //TxtEmail.Text = null;
            //TxtSenha.Text = null;
            //TxtConfirmarSenha.Text = null;
            //ChkSSL.Checked = true;

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((RequisicaoItem)args);
            EditItemViewLayout.ViewModal.Show();
        }

        protected void ViewLayout_Carregar(object args)
        {

            RequisicaoItem = (RequisicaoItem)args;

            //TxtSmtp.Text = UsuarioEmail.Smtp.ToStringOrNull();
            //TxtPorta.Text = UsuarioEmail.Porta.ToStringOrNull();
            //TxtEmail.Text = UsuarioEmail.Email.ToStringOrNull();
            //TxtSenha.Text = UsuarioEmail.Senha.ToStringOrNull();
            //TxtConfirmarSenha.Text = UsuarioEmail.Senha.ToStringOrNull();
            //ChkSSL.Checked = UsuarioEmail.Ssl.ToBoolean();

        }

        protected async Task ViewLayout_Salvar()
        {


            var Request = new Request();


            RequisicaoItem.Quantidade = TxtQuantidade.Text.ToDecimalOrNull();



            var Where = new List<Where>();

            Where.Add(new Where("EstoqueMovimentoItemEntrada.CodigoBarra = @0", TxtCodigoBarras.Text));

            Request.Parameters.Add(new Parameters("Where", Where));

            var EstoqueMovimentoItem = await HelpHttp.Send<EstoqueMovimentoItem>(Http, "api/Estoque/GetEstoqueMovimentoItem", Request);


            if (EstoqueMovimentoItem == null)
            {
                throw new EmptyException("Código de barras não encontrado!", TxtCodigoBarras.Element);
            }

            RequisicaoItem.ProdutoID = EstoqueMovimentoItem.ProdutoID;
            RequisicaoItem.Produto = EstoqueMovimentoItem.Produto;
            RequisicaoItem.EstoqueMovimentoItemEntradaID = EstoqueMovimentoItem.EstoqueMovimentoItemEntradaID;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(RequisicaoItem);
            }

            ListItemViewLayout.GridViewItem.Refresh();
            ListItemViewLayout.Refresh();

            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Delete(object args)
        {
            foreach(var item in (List<RequisicaoItem>)args) ListItemViewLayout.ListItemView.Remove(item);
            EditItemViewLayout.ViewModal.Hide();
        }

    }
}