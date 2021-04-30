using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Login.Entrar
{
    public class ViewLoginPage: HelpComponent
    {

        public TextBox TxtName { get; set; }

        public DropDownList DplEmpresa { get; set; }
        public TextBox TxtUsuario { get; set; }

        public CheckBox ChkManterConectado { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            

            var Dominio = await HelpConexao.GetDominio(JSRuntime);

            if (string.IsNullOrEmpty(Dominio.Name))
            {
                NavigationManager.NavigateTo("Login/Conexao");
                return;
            }

            if (await HelpParametros.VerificarUsuarioLogado(JSRuntime))
            {
                NavigationManager.NavigateTo("");
                return;
            }

            DplEmpresa.Add(null, "[Selecione]");

            TxtName.Text = Dominio.Name;

            TxtUsuario.Focus();

        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await base.OnAfterRenderAsync(firstRender);

        //    if (firstRender)
        //    {
        //        DplEmpresa.Add(null, "[Selecione]");
        //    }
        //}

        protected async Task BtnAlterarDominio_Click()
        {
            await HelpConexao.SetName(JSRuntime, null);
            NavigationManager.NavigateTo("Login/Conexao");
        }

        protected async Task BtnEntrar_Click()
        {

            HelpParametros.Parametros.UsuarioLogado = new Usuario() { };

            if (ChkManterConectado.Checked)
            {
                var UsuarioID = "1"; //HelpCriptografia.Criptografar("1");
                var EmpresaID = "1"; //HelpCriptografia.Criptografar("1");
                await HelpCookie.Set(JSRuntime, "ManterConectado", UsuarioID + "§" + EmpresaID, 30);
            }

            NavigationManager.NavigateTo("");

        }

    }
}
