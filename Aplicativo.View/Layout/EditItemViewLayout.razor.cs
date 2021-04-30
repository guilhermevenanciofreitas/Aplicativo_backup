using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Core.Component;
using Skclusive.Material.Button;
using Skclusive.Material.Menu;
using Skclusive.Material.Tab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout
{

    public class BtnView
    {
        public Button Button { get; set; }
        public string Label { get; set; }
        public string Width { get; set; } = "110px";
        public bool Disabled { get; set; } = false;
        public bool Visible { get; set; } = true;
    }

    public enum ItemViewMode
    {
        New,
        Edit,
    }

    public class EditItemViewLayoutPage<TValue> : HelpComponent
    {


        public TValue ViewModel = Activator.CreateInstance<TValue>();

        [Parameter]
        public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }

        public ViewModal ViewModal { get; set; }

        public ItemViewMode ItemViewMode { get; set; }

        [Parameter] public string Title { get; set; }
        [Parameter] public string Width { get; set; }
        [Parameter] public bool Simples { get; set; } = false;

        [Parameter] public EventCallback OnPageLoad { get; set; }
        [Parameter] public EventCallback OnPageHide { get; set; }

        [Parameter] public EventCallback OnLimpar { get; set; }
        [Parameter] public EventCallback<object> OnCarregar { get; set; }
        [Parameter] public EventCallback OnSalvar { get; set; }
        [Parameter] public EventCallback OnExcluir { get; set; }

        public BtnView BtnLimpar { get; set; } = new BtnView() { Label = "Limpar" };
        public BtnView BtnSalvar { get; set; } = new BtnView() { Label = "Salvar" };
        public BtnView BtnExcluir { get; set; } = new BtnView() { Label = "Excluir" };

        public List<ItemViewButton> ItemViewButtons { get; set; } = new List<ItemViewButton>();

        [Parameter] public RenderFragment View { get; set; }

        protected void ViewModal_PageLoad()
        {
            OnPageLoad.InvokeAsync(null);
        }

        protected void ViewModal_PageHide()
        {
            OnPageHide.InvokeAsync(null);
        }

        public void LimparCampos(object Page)
        {

            var TextBox = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(TextBox)).ToList();

            var TextArea = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(TextArea)).ToList();

            var NumericBox = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(NumericBox)).ToList();

            var CheckBox = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(CheckBox)).ToList();

            var DatePicker = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(DatePicker)).ToList();

            var DateTimePicker = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(DateTimePicker)).ToList();

            var DropDownList = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(DropDownList)).ToList();


            foreach (var item in TextBox)
            {
                ((TextBox)item.GetValue(Page)).Text = null;
            }

            foreach (var item in TextArea)
            {
                ((TextArea)item.GetValue(Page)).Text = null;
            }

            foreach (var item in NumericBox)
            {
                ((NumericBox)item.GetValue(Page)).Value = 0;
            }

            foreach (var item in CheckBox)
            {
                ((CheckBox)item.GetValue(Page)).Checked = false;
            }

            foreach (var item in DatePicker)
            {
                ((DatePicker)item.GetValue(Page)).Value = null;
            }

            foreach (var item in DateTimePicker)
            {
                ((DateTimePicker)item.GetValue(Page)).Value = null;
            }

            foreach (var item in DropDownList)
            {
                ((DropDownList)item.GetValue(Page)).SelectedValue = null;
            }

        }

        public void Disable(object Page, bool Disable)
        {

            var TextBox = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(TextBox)).ToList();

            var TextArea = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(TextArea)).ToList();

            var CheckBox = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(CheckBox)).ToList();

            var DatePicker = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(DatePicker)).ToList();

            var DateTimePicker = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(DateTimePicker)).ToList();

            var DropDownList = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(DropDownList)).ToList();


            foreach (var item in TextBox)
            {
                ((TextBox)item.GetValue(Page)).ReadOnly = Disable;
            }

            foreach (var item in TextArea)
            {
                ((TextArea)item.GetValue(Page)).ReadOnly = Disable;
            }

            foreach (var item in CheckBox)
            {
                ((CheckBox)item.GetValue(Page)).ReadOnly = Disable;
            }

            foreach (var item in DatePicker)
            {
                ((DatePicker)item.GetValue(Page)).ReadOnly = Disable;
            }

            foreach (var item in DateTimePicker)
            {
                ((DateTimePicker)item.GetValue(Page)).ReadOnly = Disable;
            }

        }

        protected async void BtnLimpar_Click()
        {
            try
            {

                ItemViewMode = ItemViewMode.New;

                ViewModel = Activator.CreateInstance<TValue>();

                await OnLimpar.InvokeAsync(null);
                StateHasChanged();

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        public async Task Carregar(object args)
        {

            await OnLimpar.InvokeAsync(null);

            if (args == null)
            {
                BtnLimpar_Click();
            }
            else
            {
                ItemViewMode = ItemViewMode.Edit;
                await OnCarregar.InvokeAsync(args);
            }

            ViewModal.Show();

            ListItemViewLayout.Refresh();
            StateHasChanged();

            //await JSRuntime.InvokeVoidAsync("window.history.pushState", "1");

        }

        protected async void BtnSalvar_Click()
        {
            try
            {
                await HelpLoading.Show(this, "Salvando...");
                await OnSalvar.InvokeAsync(null);
                await ListItemViewLayout.BtnPesquisar_Click();

                if (Simples)
                {
                    if (ItemViewMode == ItemViewMode.New)
                    {
                        await ListItemViewLayout.ShowToast("Informação:", "Adicionado com sucesso!", "e-toast-success", "e-success toast-icons");
                    }
                    else
                    {
                        await ListItemViewLayout.ShowToast("Informação:", "Alterado com sucesso!", "e-toast-success", "e-success toast-icons");
                    }
                }
                else
                {
                    await ListItemViewLayout.ShowToast("Informação:", "Salvo com sucesso!", "e-toast-success", "e-success toast-icons");
                }

                if (HelpParametros.Template == Template.Desktop)
                {
                    ListItemViewLayout.GridViewItem.Refresh();
                }

                StateHasChanged();

            }
            catch (HelpEmptyException)
            {
                //await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            catch (Exception ex)
            {
                ViewModal.Show();
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        protected async void BtnExcluir_Click()
        {
            try
            {

                var confirm = await JSRuntime.InvokeAsync<bool>("confirm", "Tem certeza que deseja excluir ?");
                if (!confirm) return;

                ViewModal.Hide();
                await HelpLoading.Show(this, "Excluindo...");
                await OnExcluir.InvokeAsync(null);
                await ListItemViewLayout.BtnPesquisar_Click();
                await ListItemViewLayout.ShowToast("Informação:", "Excluído com sucesso!", "e-toast-success", "e-success toast-icons");

                ListItemViewLayout.GridViewItem.Refresh();
                ListItemViewLayout.Refresh();

                StateHasChanged();

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                ViewModal.Show();
            }
            finally
            {
                await HelpLoading.Hide(this);
            }
        }

        public List<TValue> SelectedValue()
        {
            if (ViewModal.Open)
            {
                return new List<TValue> { ViewModel };
            }
            else
            {
                return ListItemViewLayout.ListItemView.Where(c => Convert.ToBoolean(c.GetType().GetProperty("Selected").GetValue(c)) == true).ToList();
            }
        }


        #region HelpHttp

        public async Task<T> Carregar<T>(HelpQuery Query) where T : class
        {

            var Request = new Request();

            Request.Parameters.Add(new Parameters("Query", Query));

            var Result = await HelpHttp.Send<List<T>>(Http, "api/Default/Query", Request);

            return Result.FirstOrDefault();

        }

        public async Task<T> Update<T>(T Object) where T : class
        {
            return (await Update(new List<T> { Object })).FirstOrDefault();
        }

        public async Task<List<T>> Update<T>(List<T> Object) where T : class
        {

            var Request = new Request();

            Request.Parameters.Add(new Parameters("Table", Object.FirstOrDefault().GetType().Name));
            Request.Parameters.Add(new Parameters(Object.FirstOrDefault().GetType().Name, Object));

            return (await HelpHttp.Send<List<T>>(Http, "api/Default/Save", Request));
        }

        public async Task Delete(object Object)
        {
            await ListItemViewLayout.Delete(new List<object> { Object });
        }

        #endregion

        #region Menu
        protected bool ItemViewButtonOpen { set; get; }

        protected IReference ItemViewOpenButtonRef { set; get; } = new Reference();

        protected void ItemViewOpen_Close(EventArgs args)
        {
            ItemViewButtonOpen = false;
        }

        protected void ItemViewOpen_Close(MenuCloseReason reason)
        {
            ItemViewButtonOpen = false;
        }

        protected void ItemViewButtonOpen_Show()
        {
            ItemViewButtonOpen = true;
        }
        #endregion

    }
}