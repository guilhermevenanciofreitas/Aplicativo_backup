using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{
    public class CheckBoxControl : HelpComponent
    {

        public ElementReference CheckBox;

        [Parameter] public string _Label { get; set; }

        [Parameter] public bool _Checked { get; set; }

        [Parameter] public EventCallback Changed { get; set; }

        public string Label
        {
            get
            {
                return _Label;
            }
            set
            {
                _Label = value;
                StateHasChanged();
            }
        }

        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                _Changed(value);
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {

                    Label = _Label;
                    Checked = _Checked;

                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                }
            }
        }

        public void Focus()
        {
            //CheckBox.Focus(JSRuntime);
        }

        protected void _Changed(bool args)
        {
            //if (args != _Checked)
            //{
            Changed.InvokeAsync(args);
            //}
        }

        protected void _Checked_Change(ChangeEventArgs args)
        {
            Checked = !Checked;
        }

    }
}
