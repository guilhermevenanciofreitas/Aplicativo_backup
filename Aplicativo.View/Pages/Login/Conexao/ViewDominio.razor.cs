using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Login.Conexao
{
    public class ViewDominioPage : HelpComponent
    {

        public TextBox TxtName { get; set; }

        protected async Task BtnConfirmar_Click()
        {
            try
            {

                var Name = TxtName.Text;

                await HelpConexao.Add(JSRuntime, new Helpers.Conexao()
                {
                    Name = Name,
                    Server = "",
                    Database = "",
                    UserId = "",
                    Password = "",
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
