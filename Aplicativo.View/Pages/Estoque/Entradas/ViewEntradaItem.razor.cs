using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Estoque.Entradas
{
    public class ViewEntradaItemPage : ComponentBase
    {

        public EstoqueMovimentoItem ViewModel { get; set; } = new EstoqueMovimentoItem();

        public ListItemViewLayout<EstoqueMovimentoItem> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public ViewPesquisa<Produto> ViewPesquisaProduto { get; set; }
        public TextBox TxtItemNF { get; set; }
        #endregion

        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {

            ViewPesquisaProduto.Where.Clear();
            ViewPesquisaProduto.AddWhere("Ativo == @0", true);

            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void BtnLimpar_Click()
        {

            ViewModel = new EstoqueMovimentoItem();

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaProduto.Clear();

            ViewPesquisaProduto.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            BtnLimpar_Click();

            if (args == null) return;

            ViewModel = (EstoqueMovimentoItem)args;

            ViewPesquisaProduto.Value = ViewModel.ProdutoID.ToStringOrNull();
            ViewPesquisaProduto.Text = ViewModel.Produto?.Descricao.ToStringOrNull();

            TxtItemNF.Text = ViewModel.NotaFiscalItem.cProd + " - " + ViewModel.NotaFiscalItem.xProd;
            //TxtSmtp.Text = ViewModel.Smtp.ToStringOrNull();
            //TxtPorta.Text = ViewModel.Porta.ToStringOrNull();
            //TxtEmail.Text = ViewModel.Email.ToStringOrNull();
            //TxtSenha.Text = ViewModel.Senha.ToStringOrNull();
            //TxtConfirmarSenha.Text = ViewModel.Senha.ToStringOrNull();
            //ChkSSL.Checked = ViewModel.Ssl.ToBoolean();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtSmtp.Text))
            //    throw new EmptyException("Informe o SMTP!", TxtSmtp.Element);

            //if (string.IsNullOrEmpty(TxtPorta.Text))
            //    throw new EmptyException("Informe a porta!", TxtPorta.Element);

            //if (string.IsNullOrEmpty(TxtEmail.Text))
            //    throw new EmptyException("Informe o email!", TxtEmail.Element);

            //if (string.IsNullOrEmpty(TxtSenha.Text))
            //    throw new EmptyException("Informe a senha!", TxtSenha.Element);

            //if (TxtSenha.Text != TxtConfirmarSenha.Text)
            //    throw new EmptyException("A confirmação da senha está diferente da senha informada!", TxtConfirmarSenha.Element);

            //ViewModel.Smtp = TxtSmtp.Text.ToStringOrNull();
            //ViewModel.Porta = TxtPorta.Text.ToIntOrNull();
            //ViewModel.Email = TxtEmail.Text.ToStringOrNull();
            //ViewModel.Senha = TxtSenha.Text.ToStringOrNull();
            //ViewModel.Ssl = ChkSSL.Checked;

            ViewModel.ProdutoID = ViewPesquisaProduto.Value.ToIntOrNull();
            ViewModel.Produto = new Produto() { Descricao = ViewPesquisaProduto.Text };

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        //protected async Task ViewPageBtnExcluir_Click()
        //{

        //    Excluir(new List<EstoqueMovimentoItem>() { ViewModel });

        //    await EditItemViewLayout.ViewModal.Hide();

        //}

        //protected void ListViewBtnExcluir_Click(object args)
        //{

        //    Excluir(((IEnumerable)args).Cast<EstoqueMovimentoItem>().ToList());

        //}

        //public void Excluir(List<EstoqueMovimentoItem> args)
        //{
        //    foreach (var item in args)
        //    {
        //        ListView.Items.Remove(item);
        //    }
        //}

        #endregion

    }
}