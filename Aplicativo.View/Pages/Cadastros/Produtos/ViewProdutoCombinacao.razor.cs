using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Produtos
{
    public class ViewProdutoCombinacaoPage : ComponentBase
    {

        public ProdutoCombinacao ViewModel { get; set; } = new ProdutoCombinacao();

        public List<Atributo> ListAtributo { get; set; } = new List<Atributo>();

        public ListItemViewLayout<ProdutoCombinacao> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TextBox TxtDescricao { get; set; }
        #endregion

        #region ListView
        protected void Page_Load()
        {

            //ViewPesquisaFornecedor.AddWhere("IsFornecedor == @0", true);
            //ViewPesquisaFornecedor.AddWhere("Ativo == @0", true);

            //DplUnidadeMedida.LoadDropDownList("UnidadeMedidaID", "Unidade", new DropDownListItem(null, "[Selecione]"), HelpParametros.Parametros.UnidadeMedida.Where(c => c.Ativo == true).ToList());

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void BtnLimpar_Click()
        {

            ViewModel = new ProdutoCombinacao();

            EditItemViewLayout.LimparCampos(this);

            TxtDescricao.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            BtnLimpar_Click();

            if (args == null)
            {

                foreach (var item in ListAtributo)
                {

                    var HelpVariacao = new HelpQuery<Variacao>();

                    HelpVariacao.AddWhere("AtributoID == @0", item.AtributoID);

                    item.Variacao = await HelpVariacao.ToList();
                    ViewModel.ProdutoCombinacaoAtributo.Add(new ProdutoCombinacaoAtributo() { AtributoID = item.AtributoID, Atributo = item });

                }

            }
            else
            {

                ViewModel = (ProdutoCombinacao)args;

                TxtDescricao.Text = ViewModel.Descricao.ToStringOrNull();

                var HelpVariacao = new HelpQuery<Variacao>();

                HelpVariacao.AddWhere("AtributoID IN (" + string.Join(",", ListAtributo.Select(c => c.AtributoID).ToArray()) + ")");

                var Variacoes = await HelpVariacao.ToList();


                foreach (var item in ListAtributo)
                {

                    item.Variacao = Variacoes.Where(c => c.AtributoID == item.AtributoID).ToList();

                    if (ViewModel.ProdutoCombinacaoAtributo.FirstOrDefault(c => c.AtributoID == item.AtributoID) == null)
                    {
                        ViewModel.ProdutoCombinacaoAtributo.Add(new ProdutoCombinacaoAtributo() { AtributoID = item.AtributoID, Atributo = item });
                    }

                }

                foreach (var item in ViewModel.ProdutoCombinacaoAtributo)
                {
                    item.OptVariacaoID = item.VariacaoID ?? 0;
                }

            }

            StateHasChanged();

            foreach (var item in ViewModel.ProdutoCombinacaoAtributo)
            {
                await App.JSRuntime.InvokeVoidAsync("Document.SetValueElementById", "DplVariacao_" + item.AtributoID, item.OptVariacaoID ?? 0);
            }


        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            ViewModel.Descricao = TxtDescricao.Text.ToStringOrNull();

            foreach (var item in ViewModel.ProdutoCombinacaoAtributo) item.VariacaoID = item.OptVariacaoID;



            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<ProdutoCombinacao>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<ProdutoCombinacao>().ToList());

        }

        public void Excluir(List<ProdutoCombinacao> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        protected void DplVariacao_Change(ProdutoCombinacaoAtributo ProdutoCombinacaoAtributo, ChangeEventArgs args)
        {
            if (args.Value.ToString() != "0")
                ProdutoCombinacaoAtributo.OptVariacaoID = Convert.ToInt32(args.Value);
        }

        #endregion

    }
}