using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.Utils.WebServices;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public class ViewPessoaEnderecoPage : ComponentBase
    {

        public PessoaEndereco ViewModel { get; set; } = new PessoaEndereco();

        public ListItemViewLayout<PessoaEndereco> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TextBoxCEP TxtCEP { get; set; }
        public TextBox TxtLogradouro { get; set; }
        public TextBox TxtNumero { get; set; }
        public TextBox TxtComplemento { get; set; }
        public TextBox TxtBairro { get; set; }
        public TextBox TxtMunicipio { get; set; }
        #endregion

        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void ViewLayout_Limpar()
        {

            ViewModel = new PessoaEndereco();

            EditItemViewLayout.LimparCampos(this);

            TxtCEP.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            ViewLayout_Limpar();

            if (args == null) return;

            ViewModel = (PessoaEndereco)args;

            TxtCEP.Text = ViewModel.Endereco.CEP.StringFormat("##.###-###");
            TxtLogradouro.Text = ViewModel.Endereco.Logradouro.ToStringOrNull();
            TxtNumero.Text = ViewModel.Endereco.Numero.ToStringOrNull();
            TxtComplemento.Text = ViewModel.Endereco.Complemento.ToStringOrNull();
            TxtBairro.Text = ViewModel.Endereco.Bairro.ToStringOrNull();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            if (string.IsNullOrEmpty(TxtCEP.Text))
            {
                throw new EmptyException("Informe o CEP!", TxtCEP.Element);
            }

            if (string.IsNullOrEmpty(TxtLogradouro.Text))
            {
                throw new EmptyException("Informe o logradouro!", TxtLogradouro.Element);
            }

            ViewModel.Endereco = new Endereco();

            ViewModel.Endereco.CEP = TxtCEP.Text.ToStringOrNull();
            ViewModel.Endereco.Logradouro = TxtLogradouro.Text.ToStringOrNull();
            ViewModel.Endereco.Numero = TxtNumero.Text.ToStringOrNull();
            ViewModel.Endereco.Complemento = TxtComplemento.Text.ToStringOrNull();
            ViewModel.Endereco.Bairro = TxtBairro.Text.ToStringOrNull();

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<PessoaEndereco>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<PessoaEndereco>().ToList());

        }

        public void Excluir(List<PessoaEndereco> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        protected void TxtCEP_Success(object args)
        {

            var ViaCEP = (ViaCEP)args;

            TxtLogradouro.Text = ViaCEP.logradouro.ToStringOrNull();
            TxtBairro.Text = ViaCEP.bairro.ToStringOrNull();

            TxtMunicipio.Text = ViaCEP.municipio.ToStringOrNull();

            TxtNumero.Focus();

        }

        protected void TxtCEP_Error(object args)
        {
            App.JSRuntime.InvokeVoidAsync("alert", args.ToString());

            TxtLogradouro.Text = null;
            TxtBairro.Text = null;

            TxtMunicipio.Text = null;

            TxtCEP.Focus();

        }

        #endregion

    }
}