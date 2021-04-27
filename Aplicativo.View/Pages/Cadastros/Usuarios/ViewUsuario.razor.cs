using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public partial class ViewUsuarioPage<TValue> : HelpComponent
    {

        [Parameter]
        public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        public EditItemViewLayout<Usuario> EditItemViewLayout { get; set; }

        #region Elements
        public TextBox TxtCodigo { get; set; }
        public TextBox TxtLogin { get; set; }
        public TextBox TxtSenha { get; set; }
        public TextBox TxtConfirmarSenha { get; set; }

        public TabSet TabSet { get; set; }

        public ViewUsuarioEmail ViewUsuarioEmail { get; set; }
        #endregion

        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            ViewUsuarioEmail.ListItemViewLayout.ListItemView = new List<UsuarioEmail>();
            ViewUsuarioEmail.ListItemViewLayout.Refresh();

            TxtLogin.Focus();

            await TabSet.Active("Principal");

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddInclude("UsuarioEmail");
            Query.AddWhere("UsuarioID == @0", ((Usuario)args)?.UsuarioID);
            Query.AddTake(1);

            EditItemViewLayout.ViewModel = await EditItemViewLayout.Carregar<Usuario>(Query);


            TxtCodigo.Text = EditItemViewLayout.ViewModel.UsuarioID.ToStringOrNull();
            TxtLogin.Text = EditItemViewLayout.ViewModel.Login.ToStringOrNull();
            TxtSenha.Text = EditItemViewLayout.ViewModel.Senha.ToStringOrNull();
            TxtConfirmarSenha.Text = EditItemViewLayout.ViewModel.Senha.ToStringOrNull();

            ViewUsuarioEmail.ListItemViewLayout.ListItemView = EditItemViewLayout.ViewModel.UsuarioEmail.ToList();
            ViewUsuarioEmail.ListItemViewLayout.Refresh();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtLogin.Text))
            {
                await TabSet.Active("Principal");
                await HelpEmptyException.New(JSRuntime, TxtLogin.Element, "Informe o login!");
            }

            if (TxtSenha.Text != TxtConfirmarSenha.Text)
            {
                await TabSet.Active("Principal");
                await HelpEmptyException.New(JSRuntime, TxtConfirmarSenha.Element, "A confirmação da senha está diferente da senha informada!");
            }


            EditItemViewLayout.ViewModel.UsuarioID = TxtCodigo.Text.ToIntOrNull();
            EditItemViewLayout.ViewModel.Login = TxtLogin.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Senha = TxtSenha.Text.ToStringOrNull();

            EditItemViewLayout.ViewModel.UsuarioEmail = ViewUsuarioEmail.ListItemViewLayout.ListItemView;


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