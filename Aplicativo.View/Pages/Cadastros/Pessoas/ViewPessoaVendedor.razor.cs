using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public class ViewPessoaVendedorPage : ComponentBase
    {

        public PessoaVendedor ViewModel { get; set; } = new PessoaVendedor();

        public ListItemViewLayout<PessoaVendedor> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        public TextBox TxtVendedor { get; set; }


        #region ListView
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

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            BtnLimpar_Click();

            await EditItemViewLayout.Show(args);

            if (args == null) return;

            ViewModel = (PessoaVendedor)args;

            TxtVendedor.Text = ViewModel.Vendedor.NomeFantasia.ToStringOrNull();

        }

        protected void BtnSalvar_Click()
        {

            var ListItemView = ListView.Items;

            ViewModel.VendedorID = null;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemView.Add(ViewModel);
            }

            ListView.Items = ListItemView;

            EditItemViewLayout.ViewModal.Hide();

        }

        protected void BtnExcluir_Click(object args)
        {

            var ListItemView = ListView.Items;

            foreach (var item in ((IEnumerable)args).Cast<PessoaVendedor>().ToList())
            {
                ListItemView.Remove(item);
            }

            ListView.Items = ListItemView;

            EditItemViewLayout.ViewModal.Hide();

        }

        #endregion
    }
}