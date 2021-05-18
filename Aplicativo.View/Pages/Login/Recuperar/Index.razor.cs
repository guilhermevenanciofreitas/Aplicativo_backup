using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Login.Recuperar
{
    public partial class IndexPage : ComponentBase
    {

        public TextBox TxtEmail { get; set; }


        protected void Component_Load()
        {

            TxtEmail.Focus();

        }

        protected async Task BtnVoltar_Click()
        {
            try
            {
                App.NavigationManager.NavigateTo("Login/Entrar");
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected void BtnConfirmar_Click()
        {

        }

    }
}