using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public class ViewPessoaContatoPage : HelpComponent
    {

        public ListItemViewLayout<PessoaContato> ListItemViewLayout { get; set; }
        public EditItemViewLayout<PessoaContato> EditItemViewLayout { get; set; }

        public TextBox TxtNome { get; set; }
        public TextBox TxtTelefone { get; set; }
        public TextBox TxtEmail { get; set; }


        protected void ViewLayout_PageLoad()
        {

        }

        protected async Task ViewLayout_Limpar()
        {
            await EditItemViewLayout.LimparCampos(this);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((PessoaContato)args);
        }

        protected void ViewLayout_Carregar(object args)
        {

            EditItemViewLayout.ViewModel = (PessoaContato)args;

            TxtNome.Text = EditItemViewLayout.ViewModel.Contato.Nome.ToStringOrNull();
            TxtTelefone.Text = EditItemViewLayout.ViewModel.Contato.Telefone.ToStringOrNull();
            TxtEmail.Text = EditItemViewLayout.ViewModel.Contato.Email.ToStringOrNull();
            
        }

        protected void ViewLayout_Salvar()
        {

            //if (string.IsNullOrEmpty(TxtCEP.Text))
            //    throw new EmptyException("Informe o CEP!", TxtCEP.Element);

            if (string.IsNullOrEmpty(TxtNome.Text))
                throw new EmptyException("Informe o nome!", TxtNome.Element);
            

            if (EditItemViewLayout.ViewModel.Contato == null)
            {
                EditItemViewLayout.ViewModel.Contato = new Contato();
            }

            EditItemViewLayout.ViewModel.Contato.Nome = TxtNome.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Contato.Telefone = TxtTelefone.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Contato.Email = TxtEmail.Text.ToStringOrNull();
            

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
            }
            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Delete(object args)
        {
            foreach(var item in (List<PessoaContato>)args) ListItemViewLayout.ListItemView.Remove(item);
            EditItemViewLayout.ViewModal.Hide();
        }

    }
}