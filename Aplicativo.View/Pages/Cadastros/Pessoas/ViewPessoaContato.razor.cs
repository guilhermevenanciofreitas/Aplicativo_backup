using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public class ViewPessoaContatoPage : ComponentBase
    {

        public PessoaContato ViewModel { get; set; } = new PessoaContato();

        public ListItemViewLayout<PessoaContato> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        public TextBox TxtNome { get; set; }
        public TextBox TxtTelefone { get; set; }
        public TextBox TxtEmail { get; set; }


        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void ViewLayout_Limpar()
        {

            ViewModel = new PessoaContato();

            EditItemViewLayout.LimparCampos(this);

            TxtNome.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            ViewLayout_Limpar();

            if (args == null) return;

            ViewModel = (PessoaContato)args;

            TxtNome.Text = ViewModel.Contato.Nome.ToStringOrNull();
            TxtTelefone.Text = ViewModel.Contato.Telefone.ToStringOrNull();
            TxtEmail.Text = ViewModel.Contato.Email.ToStringOrNull();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            if (string.IsNullOrEmpty(TxtNome.Text))
            {
                throw new EmptyException("Informe o nome!", TxtNome.Element);
            }

            ViewModel.Contato = new Contato();

            ViewModel.Contato.Nome = TxtNome.Text.ToStringOrNull();
            ViewModel.Contato.Telefone = TxtTelefone.Text.ToStringOrNull();
            ViewModel.Contato.Email = TxtEmail.Text.ToStringOrNull();

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<PessoaContato>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<PessoaContato>().ToList());

        }

        public void Excluir(List<PessoaContato> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        #endregion
    }
}