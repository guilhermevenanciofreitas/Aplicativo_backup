using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.Utils.WebServices;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout;
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

        public ListItemViewLayout ListItemViewLayout { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        public TextBoxCEP TxtCEP { get; set; }
        public TextBox TxtLogradouro { get; set; }
        public TextBox TxtNumero { get; set; }
        public TextBox TxtComplemento { get; set; }
        public TextBox TxtBairro { get; set; }
        public TextBox TxtMunicipio { get; set; }


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

            TxtCEP.Text = null;

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            ViewLayout_Limpar();

            await EditItemViewLayout.Show(args);

            if (args == null) return;

            ViewModel = (PessoaEndereco)args;

            TxtCEP.Text = ViewModel.Endereco.CEP.StringFormat("##.###-###"); //ViewModel.Endereco.CEP.ToStringOrNull();
            TxtLogradouro.Text = ViewModel.Endereco.Logradouro.ToStringOrNull();
            TxtNumero.Text = ViewModel.Endereco.Numero.ToStringOrNull();
            TxtComplemento.Text = ViewModel.Endereco.Complemento.ToStringOrNull();
            TxtBairro.Text = ViewModel.Endereco.Bairro.ToStringOrNull();

        }

        protected void ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtCEP.Text))
            {
                throw new EmptyException("Informe o CEP!", TxtCEP.Element);
            }

            if (string.IsNullOrEmpty(TxtLogradouro.Text))
            {
                throw new EmptyException("Informe o logradouro!", TxtLogradouro.Element);
            }

            var ListItemView = ListItemViewLayout.ListItemView;

            ViewModel.Endereco = new Endereco();

            ViewModel.Endereco.CEP = TxtCEP.Text.ToStringOrNull();
            ViewModel.Endereco.Logradouro = TxtLogradouro.Text.ToStringOrNull();
            ViewModel.Endereco.Numero = TxtNumero.Text.ToStringOrNull();
            ViewModel.Endereco.Complemento = TxtComplemento.Text.ToStringOrNull();
            ViewModel.Endereco.Bairro = TxtBairro.Text.ToStringOrNull();

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemView.Add(ViewModel);
            }

            ListItemViewLayout.ListItemView = ListItemView;

            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Excluir(object args)
        {

            var ListItemView = ListItemViewLayout.ListItemView;

            foreach (var item in ((IEnumerable)args).Cast<PessoaEndereco>().ToList())
            {
                ListItemView.Remove(item);
            }

            ListItemViewLayout.ListItemView = ListItemView;

            EditItemViewLayout.ViewModal.Hide();

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

        //public ListItemViewLayout<PessoaEndereco> ListItemViewLayout { get; set; }
        //public EditItemViewLayout<PessoaEndereco> EditItemViewLayout { get; set; }

        //public TextBox TxtCEP { get; set; }
        //public TextBox TxtLogradouro { get; set; }
        //public TextBox TxtNumero { get; set; }
        //public TextBox TxtComplemento { get; set; }
        //public TextBox TxtBairro { get; set; }
        //public TextBox TxtMunicipio { get; set; }


        //protected void ViewLayout_PageLoad()
        //{

        //}

        //protected void ViewLayout_Limpar()
        //{
        //    EditItemViewLayout.LimparCampos(this);
        //}

        //protected async Task ViewLayout_ItemView(object args)
        //{
        //    await EditItemViewLayout.Carregar((PessoaEndereco)args);
        //}

        //protected void ViewLayout_Carregar(object args)
        //{

        //    EditItemViewLayout.ViewModel = (PessoaEndereco)args;

        //    TxtCEP.Text = EditItemViewLayout.ViewModel.Endereco.CEP.ToStringOrNull();
        //    TxtLogradouro.Text = EditItemViewLayout.ViewModel.Endereco.Logradouro.ToStringOrNull();
        //    TxtNumero.Text = EditItemViewLayout.ViewModel.Endereco.Numero.ToStringOrNull();
        //    TxtComplemento.Text = EditItemViewLayout.ViewModel.Endereco.Complemento.ToStringOrNull();
        //    TxtBairro.Text = EditItemViewLayout.ViewModel.Endereco.Bairro.ToStringOrNull();

        //}

        //protected async Task ViewLayout_Salvar()
        //{

        //    //if (string.IsNullOrEmpty(TxtCEP.Text))
        //    //    throw new EmptyException("Informe o CEP!", TxtCEP.Element);

        //    if (string.IsNullOrEmpty(TxtLogradouro.Text))
        //    {
        //        //await HelpEmptyException.New(JSRuntime, TxtLogradouro.Element, "Informe o logradouro!");
        //    }


        //    if (EditItemViewLayout.ViewModel.Endereco == null)
        //    {
        //        EditItemViewLayout.ViewModel.Endereco = new Endereco();
        //    }

        //    EditItemViewLayout.ViewModel.Endereco.CEP = TxtCEP.Text.ToStringOrNull();
        //    EditItemViewLayout.ViewModel.Endereco.Logradouro = TxtLogradouro.Text.ToStringOrNull();
        //    EditItemViewLayout.ViewModel.Endereco.Numero = TxtNumero.Text.ToStringOrNull();
        //    EditItemViewLayout.ViewModel.Endereco.Complemento = TxtComplemento.Text.ToStringOrNull();
        //    EditItemViewLayout.ViewModel.Endereco.Bairro = TxtBairro.Text.ToStringOrNull();


        //    if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
        //    {
        //        ListItemViewLayout.ListItemView.Add(EditItemViewLayout.ViewModel);
        //    }
        //    EditItemViewLayout.ViewModal.Hide();

        //}

        //protected void ViewLayout_Delete(object args)
        //{
        //    foreach(var item in (List<PessoaEndereco>)args) ListItemViewLayout.ListItemView.Remove(item);
        //    EditItemViewLayout.ViewModal.Hide();
        //}

    }
}