using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Login
{
    public class ViewConexaoPage : ComponentBase
    {

        public TextBox TxtName { get; set; }

        public TextBox TxtServer { get; set; }
        public TextBox TxtDatabase { get; set; }
        public TextBox TxtUserId { get; set; }
        public TextBox TxtPassword { get; set; }

        protected void Component_Load()
        {
            TxtName.Focus();
        }

        protected async Task BtnConfirmar_Click()
        {
            try
            {

                var Name = TxtName.Text;

                await HelpConexao.Add(new Conexao() {
                    Name = Name,
                    Server = TxtServer.Text,
                    Database = TxtDatabase.Text,
                    UserId = TxtUserId.Text,
                    Password = TxtPassword.Text,
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
