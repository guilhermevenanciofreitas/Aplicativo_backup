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
    public class ViewProdutoAtributoPage : ComponentBase
    {

        public ProdutoAtributo ViewModel { get; set; } = new ProdutoAtributo();

        public ListItemViewLayout<ProdutoAtributo> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        [Parameter] public EventCallback OnSave { get; set; }

        #region Elements
        public ViewPesquisa<Atributo> ViewPesquisaAtributo { get; set; }
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

            ViewModel = new ProdutoAtributo();

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaAtributo.Clear();

            //TxtCodigo.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            BtnLimpar_Click();

            if (args == null) return;

            ViewModel = (ProdutoAtributo)args;

            ViewPesquisaAtributo.Value = ViewModel.AtributoID.ToStringOrNull();
            ViewPesquisaAtributo.Text = ViewModel.Atributo?.Descricao.ToStringOrNull();
            
        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            ViewModel.AtributoID = ViewPesquisaAtributo.Value.ToIntOrNull();
            ViewModel.Atributo = new Atributo() { AtributoID = ViewModel.AtributoID, Descricao = ViewPesquisaAtributo.Text };

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

            await OnSave.InvokeAsync(ListView.Items.Select(c => c.Atributo).ToList());

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<ProdutoAtributo>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<ProdutoAtributo>().ToList());

        }

        public void Excluir(List<ProdutoAtributo> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        #endregion

    }
}