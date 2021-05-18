using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public partial class ViewUsuarioPage : ComponentBase
    {

        [Parameter] public bool BtnLimpar { get; set; } = true;
        [Parameter] public bool BtnExcluir { get; set; } = true;

        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TextBox TxtCodigo { get; set; }
        public TextBox TxtLogin { get; set; }
        public TextBox TxtSenha { get; set; }
        public TextBox TxtConfirmarSenha { get; set; }

        public TabSet TabSet { get; set; }

        public ViewUsuarioEmail ViewUsuarioEmail { get; set; }
        #endregion

        protected async Task ViewLayout_Load(object args)
        {

            await ViewLayout_Limpar();

            if (args == null) return;

            var Query = new HelpQuery<Usuario>();

            Query.AddInclude("UsuarioEmail");
            Query.AddWhere("UsuarioID == @0", ((Usuario)args).UsuarioID);
            
            var ViewModel = await Query.FirstOrDefault();

            TxtCodigo.Text = ViewModel.UsuarioID.ToStringOrNull();
            TxtLogin.Text = ViewModel.Login.ToStringOrNull();
            TxtSenha.Text = ViewModel.Senha.ToStringOrNull();
            TxtConfirmarSenha.Text = ViewModel.Senha.ToStringOrNull();

            ViewUsuarioEmail.ListItemViewLayout.ListItemView = ViewModel.UsuarioEmail.Cast<object>().ToList();
            
        }

        protected async Task ViewLayout_Limpar()
        {

            EditItemViewLayout.LimparCampos(this);

            ViewUsuarioEmail.ListItemViewLayout.ListItemView = new List<object>();
            
            await TabSet.Active("Principal");

            TxtLogin.Focus();

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtLogin.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe o login!", TxtLogin.Element);
            }

            if (TxtSenha.Text != TxtConfirmarSenha.Text)
            {
                await TabSet.Active("Principal");
                throw new EmptyException("A confirmação da senha está diferente da senha informada!", TxtConfirmarSenha.Element);
            }


            var ViewModel = new Usuario();

            ViewModel.UsuarioID = TxtCodigo.Text.ToIntOrNull();
            ViewModel.Login = TxtLogin.Text.ToStringOrNull();
            ViewModel.Senha = TxtSenha.Text.ToStringOrNull();

            ViewModel.UsuarioEmail = ViewUsuarioEmail.ListItemViewLayout.ListItemView.Cast<UsuarioEmail>().ToList();


            var Query = new HelpQuery<Usuario>();

            ViewModel = await Query.Update(ViewModel);


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                TxtCodigo.Text = ViewModel.UsuarioID.ToStringOrNull();
            }
            else
            {
                EditItemViewLayout.ViewModal.Hide();
            }

        }

        protected async Task ViewLayout_Excluir()
        {

            await Excluir(new List<int> { TxtCodigo.Text.ToInt() });

            EditItemViewLayout.ViewModal.Hide();

        }

        public async Task Excluir(List<int> args)
        {

            var Query = new HelpQuery<Usuario>();

            Query.AddWhere("UsuarioID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            await Query.Update(ViewModel, false);

        }

    }
}