using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Login.Conexao
{
    public class ViewConexaoPage : HelpComponent
    {

        public TextBox TxtName { get; set; }

        public TextBox TxtServer { get; set; }
        public TextBox TxtDatabase { get; set; }
        public TextBox TxtUserId { get; set; }
        public TextBox TxtPassword { get; set; }

        protected async Task BtnConfirmar_Click()
        {
            try
            {

                var Name = TxtName.Text;

                await HelpConexao.Add(JSRuntime, new Helpers.Conexao() {
                    Name = Name,
                    Server = TxtServer.Text,
                    Database = TxtDatabase.Text,
                    UserId = TxtUserId.Text,
                    Password = TxtPassword.Text,
                });

                await HelpConexao.SetName(JSRuntime, Name);

                NavigationManager.NavigateTo("Login/Entrar");

            }
            catch (Exception ex)
            {
                await HelpErro.Show(this, ex);
            }
        }

    }
}
