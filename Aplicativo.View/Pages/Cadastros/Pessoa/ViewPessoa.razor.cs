using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoa
{
    public partial class ViewPessoaPage<TValue> : HelpComponent
    {

        [Parameter] public string Title { get; set; }

        [Parameter]
        public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        public EditItemViewLayout<Utils.Models.Pessoa> EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtRazaoSocial { get; set; }


        public ViewPessoaEndereco ViewPessoaEndereco { get; set; }

        #endregion

        protected void ViewLayout_PageLoad()
        {

        }
        
        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            ViewPessoaEndereco.ListItemViewLayout.ListItemView = new List<PessoaEndereco>();
            ViewPessoaEndereco.ListItemViewLayout.Refresh();

            TxtRazaoSocial.Focus();

            await TabSet.Active("Principal");

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddInclude("PessoaEndereco");
            Query.AddInclude("PessoaEndereco.Endereco");
            Query.AddWhere("PessoaID == @0", ((Utils.Models.Pessoa)args)?.PessoaID);
            Query.AddTake(1);

            EditItemViewLayout.ViewModel = await EditItemViewLayout.Carregar<Utils.Models.Pessoa>(Query);


            TxtCodigo.Text = EditItemViewLayout.ViewModel.PessoaID.ToStringOrNull();
            TxtRazaoSocial.Text = EditItemViewLayout.ViewModel.RazaoSocial.ToStringOrNull();


            ViewPessoaEndereco.ListItemViewLayout.ListItemView = EditItemViewLayout.ViewModel.PessoaEndereco.ToList();
            ViewPessoaEndereco.ListItemViewLayout.Refresh();


            //await JSRuntime.InvokeVoidAsync("window.history.pushState", null, null, Path.Combine(NavigationManager.ToBaseRelativePath(NavigationManager.Uri), EditItemViewLayout.ViewModel.PessoaID.ToString()));

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtRazaoSocial.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe a razão social!", TxtRazaoSocial.Element);
            }


            EditItemViewLayout.ViewModel.PessoaID = TxtCodigo.Text.ToIntOrNull();
            EditItemViewLayout.ViewModel.RazaoSocial = TxtRazaoSocial.Text.ToStringOrNull();

            EditItemViewLayout.ViewModel.PessoaEndereco = ViewPessoaEndereco.ListItemViewLayout.ListItemView;



            EditItemViewLayout.ViewModel = await EditItemViewLayout.Update(EditItemViewLayout.ViewModel);


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
                await EditItemViewLayout.Carregar(EditItemViewLayout.ViewModel);
            else
                EditItemViewLayout.ViewModal.Hide();
          
            
        }

        protected async Task ViewLayout_Excluir()
        {
            await EditItemViewLayout.Delete(EditItemViewLayout.ViewModel);
        }
    }
}