using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Core.Component;
using Skclusive.Material.Button;
using Skclusive.Material.Menu;
using Skclusive.Material.Tab;
using Syncfusion.Blazor.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        [Parameter] public string Width { get; set; }

        [Parameter] public int ZIndex { get; set; } = 8000;

        [Parameter] public RenderFragment BtnLimpar { get; set; }
        [Parameter] public RenderFragment BtnSalvar { get; set; }
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

            ViewModal.Show();

            await OnLoad.InvokeAsync(args);

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

    }
}