using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.JSInterop;
using Skclusive.Material.Icon;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public class ViewUsuarioEmailPage : HelpComponent
    {

        public ListItemViewLayout<UsuarioEmail> ListItemViewLayout { get; set; }
        public EditItemViewLayout<UsuarioEmail> EditItemViewLayout { get; set; }

        protected TextBox TxtEmail { get; set; }


        protected UsuarioEmail UsuarioEmail { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

            }
        }

        protected void ViewLayout_Limpar()
        {

            UsuarioEmail = new UsuarioEmail();

            TxtEmail.Text = null;

        }

        protected async Task ViewLayout_ItemView(object args)
        {
            await EditItemViewLayout.Carregar((UsuarioEmail)args);
            EditItemViewLayout.ViewModal.Show();
        }

        protected void ViewLayout_Carregar(object args)
        {

            UsuarioEmail = (UsuarioEmail)args;

            TxtEmail.Text = UsuarioEmail.Email;

        }

        protected void ViewLayout_Salvar()
        {

            UsuarioEmail.Email = TxtEmail.Text;

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListItemViewLayout.ListItemView.Add(UsuarioEmail);
            }
            EditItemViewLayout.ViewModal.Hide();

        }

        protected void ViewLayout_Delete(object args)
        {
            foreach(var item in (List<UsuarioEmail>)args) ListItemViewLayout.ListItemView.Remove(item);
            EditItemViewLayout.ViewModal.Hide();
        }

    }
}