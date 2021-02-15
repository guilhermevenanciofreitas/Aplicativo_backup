using Aplicativo.Utils;
using Aplicativo.Utils.Model;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros
{
    public class ListUsuarioPage : HelpPage
    {

        protected ViewModal ViewModalFiltro;
        protected ViewModal ViewModalUsuario;
        protected ViewUsuario ViewUsuario;

        public List<ItemView> ListItemView { get; set; } = new List<ItemView>();

        protected async void BtnFiltro_Click()
        {
            try
            {
                ViewModalFiltro.Show();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        protected async void BtnCarregar_Click()
        {
            try
            {
                await HelpLoading.Show(this, "Carregando...");
                await Carregar();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        private async Task Carregar()
        {
            var Request = new Request();

            var content = await HelpHttp.Send<List<Usuario>>(Http, "api/Usuario/GetAll", Request);

            ListItemView = content.Select(c =>
            new ItemView()
            {
                Bool01 = false,
                Inteiro01 = c.UsuarioID,
                Descricao01 = c.Nome,
            }).ToList();

            StateHasChanged();
        }

        protected async void BtnNovo_Click()
        {
            try
            {
                await ViewUsuario.Carregar(null);
                ViewModalUsuario.Show();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        public async Task BtnExcluir_Click()
        {
            try
            {

                await HelpLoading.Show(this, "Excluindo...");

                var Request = new Request();

                var Usuarios = ListItemView.Where(c => c.Bool01 == true).Select(c => c.Inteiro01);

                Request.Parameters.Add(new Parameters("Usuarios", Usuarios));

                await HelpHttp.Send(Http, "api/Usuario/Delete", Request);

                await Carregar();

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        protected async void ViewUsuario_Save()
        {
            try
            {
                ViewModalUsuario.Hide();
                await Carregar();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        protected async void ListItemView_Press(ItemView ItemView)
        {
            if (!ListItemView.Any(c => c.Bool01 == true))
            {
                try
                {
                    await HelpLoading.Show(this, "Carregando...");

                    await ViewUsuario.Carregar(ItemView.Inteiro01);
                    ViewModalUsuario.Show();

                    StateHasChanged();

                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
                }
                finally
                {
                    await HelpLoading.Hide(this);
                }
            }
            else
            {
                ItemView.Bool01 = !ItemView.Bool01;
            }
        }

        protected void ListItemView_LongPress(ItemView ItemView)
        {
            ItemView.Bool01 = !ItemView.Bool01;
        }

        protected void ListItemView_Unmake()
        {
            foreach(var item in ListItemView)
            {
                item.Bool01 = false;
            }
        }

    }
}