using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Login
{
    public class ViewDominioPage : ComponentBase
    {

        public TextBox TxtName { get; set; }

        protected void Component_Load()
        {
            TxtName.Focus();
        }

        protected async Task BtnConfirmar_Click()
        {
            try
            {

                var Name = TxtName.Text;

                await HelpConexao.Add(new Conexao()
                {
                    Name = Name,
                    Server = "",
                    Database = "",
                    UserId = "",
                    Password = "",
                });

                await HelpConexao.SetName(Name);

                App.NavigationManager.NavigateTo("Login/Entrar");

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}