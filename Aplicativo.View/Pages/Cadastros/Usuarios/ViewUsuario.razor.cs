using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public partial class ViewUsuarioPage : ComponentBase
    {

        public Usuario ViewModel = new Usuario();

        [Parameter] public bool BtnLimpar { get; set; } = true;
        [Parameter] public bool BtnExcluir { get; set; } = true;

        [Parameter] public ListItemViewLayout<Usuario> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TextBox TxtCodigo { get; set; }
        public TextBox TxtLogin { get; set; }
        public ViewPesquisa<Pessoa> ViewPesquisaFuncionario { get; set; }

        public TextBox TxtSenha { get; set; }
        public TextBox TxtConfirmarSenha { get; set; }

        public TabSet TabSet { get; set; }

        public ViewUsuarioEmail ViewUsuarioEmail { get; set; }
        #endregion

        protected async Task Page_Load(object args)
        {

            ViewPesquisaFuncionario.AddWhere("IsFuncionario == @0", true);
            ViewPesquisaFuncionario.AddWhere("Ativo == @0", true);

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<Usuario>();

            Query.AddInclude("Funcionario");
            Query.AddInclude("UsuarioEmail");
            Query.AddWhere("UsuarioID == @0", ((Usuario)args).UsuarioID);
            
            ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            TxtCodigo.Text = ViewModel.UsuarioID.ToStringOrNull();
            TxtLogin.Text = ViewModel.Login.ToStringOrNull();

            ViewPesquisaFuncionario.Value = ViewModel.FuncionarioID.ToStringOrNull();
            ViewPesquisaFuncionario.Text = ViewModel.Funcionario?.NomeFantasia.ToStringOrNull();

            TxtSenha.Text = ViewModel.Senha.ToStringOrNull();
            TxtConfirmarSenha.Text = ViewModel.Senha.ToStringOrNull();

            ViewUsuarioEmail.ListView.Items = ViewModel.UsuarioEmail.ToList();
            
        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaFuncionario.Clear();

            ViewUsuarioEmail.ListView.Items = new List<UsuarioEmail>();

            await TabSet.Active("Principal");

            TxtLogin.Focus();

        }

        protected async Task BtnSalvar_Click()
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

            ViewModel.UsuarioID = TxtCodigo.Text.ToIntOrNull();
            ViewModel.Login = TxtLogin.Text.ToStringOrNull();

            ViewModel.FuncionarioID = ViewPesquisaFuncionario.Value.ToIntOrNull();
            ViewModel.Funcionario = null;

            ViewModel.Senha = TxtSenha.Text.ToStringOrNull();

            ViewModel.UsuarioEmail = ViewUsuarioEmail.ListView.Items.ToList();

            foreach(var item in ViewModel.UsuarioEmail)
            {

            }


            var Query = new HelpQuery<Usuario>();

            ViewModel = await Query.Update(ViewModel);

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                await Page_Load(ViewModel);
            }
            else
            {
                await EditItemViewLayout.ViewModal.Hide();
            }

        }

        protected async Task BtnExcluir_Click()
        {

            await Excluir(new List<int> { TxtCodigo.Text.ToInt() });

            await EditItemViewLayout.ViewModal.Hide();

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