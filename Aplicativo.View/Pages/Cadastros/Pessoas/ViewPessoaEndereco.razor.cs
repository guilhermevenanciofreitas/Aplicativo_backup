using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public class ViewPessoaEnderecoPage : HelpComponent
    {

        public ListItemViewLayout<PessoaEndereco> ListItemViewLayout { get; set; }
        public EditItemViewLayout<PessoaEndereco> EditItemViewLayout { get; set; }

        public TextBox TxtCEP { get; set; }
        public TextBox TxtLogradouro { get; set; }
        public TextBox TxtNumero { get; set; }
        public TextBox TxtComplemento { get; set; }
        public TextBox TxtBairro { get; set; }
        public TextBox TxtMunicipio { get; set; }


        protected void ViewLayout_PageLoad()
        {

        }

        protected void ViewLayout_Limpar()
        {
            EditItemViewLayout.LimparCampos(this);
        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((PessoaEndereco)args);
        }

        protected void ViewLayout_Carregar(object args)
        {

            EditItemViewLayout.ViewModel = (PessoaEndereco)args;

            TxtCEP.Text = EditItemViewLayout.ViewModel.Endereco.CEP.ToStringOrNull();
            TxtLogradouro.Text = EditItemViewLayout.ViewModel.Endereco.Logradouro.ToStringOrNull();
            TxtNumero.Text = EditItemViewLayout.ViewModel.Endereco.Numero.ToStringOrNull();
            TxtComplemento.Text = EditItemViewLayout.ViewModel.Endereco.Complemento.ToStringOrNull();
            TxtBairro.Text = EditItemViewLayout.ViewModel.Endereco.Bairro.ToStringOrNull();

        }

        protected void ViewLayout_Salvar()
        {

            //if (string.IsNullOrEmpty(TxtCEP.Text))
            //    throw new EmptyException("Informe o CEP!", TxtCEP.Element);

            if (string.IsNullOrEmpty(TxtLogradouro.Text))
                throw new EmptyException("Informe o logradouro!", TxtLogradouro.Element);
            

            if (EditItemViewLayout.ViewModel.Endereco == null)
            {
                EditItemViewLayout.ViewModel.Endereco = new Endereco();
            }

            EditItemViewLayout.ViewModel.Endereco.CEP = TxtCEP.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Endereco.Logradouro = TxtLogradouro.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Endereco.Numero = TxtNumero.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Endereco.Complemento = TxtComplemento.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Endereco.Bairro = TxtBairro.Text.ToStringOrNull();


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
            }
            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Delete(object args)
        {
            foreach(var item in (List<PessoaEndereco>)args) ListItemViewLayout.ListItemView.Remove(item);
            EditItemViewLayout.ViewModal.Hide();
        }

    }
}