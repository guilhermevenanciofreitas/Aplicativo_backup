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

namespace Aplicativo.View.Pages.Fiscal.NotasFiscais
{
    public partial class ViewNotaFiscalPage : ComponentBase
    {

        public NotaFiscal ViewModel = new NotaFiscal();

        [Parameter] public ListItemViewLayout<NotaFiscal> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TextBox TxtCodigo { get; set; }
        public TextBox TxtLogin { get; set; }
        public TextBox TxtSenha { get; set; }
        public TextBox TxtConfirmarSenha { get; set; }

        public TabSet TabSet { get; set; }

        //public ViewUsuarioEmail ViewUsuarioEmail { get; set; }
        #endregion

        protected async Task Page_Load(object args)
        {

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<NotaFiscal>();

            Query.AddInclude("UsuarioEmail");
            Query.AddWhere("UsuarioID == @0", ((NotaFiscal)args).NotaFiscalID);
            
            ViewModel = await Query.FirstOrDefault();

            //TxtCodigo.Text = ViewModel.UsuarioID.ToStringOrNull();
            //TxtLogin.Text = ViewModel.Login.ToStringOrNull();
            //TxtSenha.Text = ViewModel.Senha.ToStringOrNull();
            //TxtConfirmarSenha.Text = ViewModel.Senha.ToStringOrNull();

            //ViewUsuarioEmail.ListView.Items = ViewModel.UsuarioEmail.ToList();
            
        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.LimparCampos(this);

            //ViewUsuarioEmail.ListView.Items = new List<UsuarioEmail>();

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

            //ViewModel.UsuarioID = TxtCodigo.Text.ToIntOrNull();
            //ViewModel.Login = TxtLogin.Text.ToStringOrNull();
            //ViewModel.Senha = TxtSenha.Text.ToStringOrNull();

            //ViewModel.UsuarioEmail = ViewUsuarioEmail.ListView.Items.ToList();


            var Query = new HelpQuery<NotaFiscal>();

            ViewModel = await Query.Update(ViewModel);

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                TxtCodigo.Text = ViewModel.NotaFiscalID.ToStringOrNull();
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