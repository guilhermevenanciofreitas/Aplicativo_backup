using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Layout.Component.ViewPage
{

    public enum ItemViewMode
    {
        New,
        Edit,
    }

    public partial class EditItemViewLayoutPage : ComponentBase
    {


        public ItemViewMode ItemViewMode { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public string Top { get; set; }
        [Parameter] public string Width { get; set; }

        [Parameter] public RenderFragment BtnLimpar { get; set; }
        [Parameter] public RenderFragment BtnSalvar { get; set; }
        [Parameter] public RenderFragment BtnCustom { get; set; }
        [Parameter] public RenderFragment BtnExcluir { get; set; }

        [Parameter] public EventCallback OnLoad { get; set; }

        public ViewModal ViewModal { get; set; }

        [Parameter] public RenderFragment View { get; set; }

        public async Task Show(object args)
        {

            if (args == null)
            {
                ItemViewMode = ItemViewMode.New;
            }
            else
            {
                ItemViewMode = ItemViewMode.Edit;
                
            }

            await OnLoad.InvokeAsync(args);

            await ViewModal.Show();

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

            var TextBoxCEP = Page.GetType().GetProperties().Where(c => c.PropertyType == typeof(TextBoxCEP)).ToList();


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

            foreach (var item in TextBoxCEP)
            {
                ((TextBoxCEP)item.GetValue(Page)).Text = null;
            }

        }

        protected void Page_Resize(object args)
        {
            StateHasChanged();
        }

        public void Refresh()
        {
            StateHasChanged();
        }

    }
}