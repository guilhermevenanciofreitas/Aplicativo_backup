using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public class ViewPessoaVendedorPage : ComponentBase
    {

        public PessoaVendedor ViewModel { get; set; } = new PessoaVendedor();

        public ListItemViewLayout<PessoaVendedor> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        
        #region Elements
        public ViewPesquisa<Pessoa> ViewPesquisaVendedor { get; set; }
        #endregion

        #region ListView
        protected void Page_Load(object args)
        {
            ViewPesquisaVendedor.AddWhere("IsFuncionario == @0", true);
            ViewPesquisaVendedor.AddWhere("Ativo == @0", true);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void BtnLimpar_Click()
        {

            ViewModel = new PessoaVendedor();

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaVendedor.Clear();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            BtnLimpar_Click();

            await EditItemViewLayout.Show(args);

            if (args == null) return;

            ViewModel = (PessoaVendedor)args;

            ViewPesquisaVendedor.Value = ViewModel.VendedorID.ToStringOrNull();
            ViewPesquisaVendedor.Text = ViewModel.Vendedor?.NomeFantasia.ToStringOrNull();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            ViewModel.VendedorID = ViewPesquisaVendedor.Value.ToIntOrNull();
            ViewModel.Vendedor = new Pessoa() { NomeFantasia = ViewPesquisaVendedor.Text };

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }


        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<PessoaVendedor>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<PessoaVendedor>().ToList());

        }

        public void Excluir(List<PessoaVendedor> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        #endregion

    }
}