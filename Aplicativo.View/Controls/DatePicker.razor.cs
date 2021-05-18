using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{

    public class DatePickerComponent : ComponentBase
    {

        public ElementReference Element;

        [Parameter] public string _Label { get; set; }
        [Parameter] public DateTime? _Value { get; set; }
        [Parameter] public string _PlaceHolder { get; set; }
        [Parameter] public bool _ReadOnly { get; set; }

        [Parameter] public EventCallback<ChangeEventArgs> OnChange { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnEnter { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnLeave { get; set; }

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

        public DateTime? Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                StateHasChanged();
            }
        }

        public string PlaceHolder
        {
            get
            {
                return _PlaceHolder;
            }
            set
            {
                _PlaceHolder = value;
                StateHasChanged();
            }
        }

        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {

                    Value = _Value;
                    PlaceHolder = _PlaceHolder;

                    StateHasChanged();

                }
                catch (Exception ex)
                {
                    await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
                }
            }
        }

        public void Focus()
        {
            Element.Focus();
        }

        protected void Change(ChangeEventArgs args)
        {
            if (args.Value.ToString() == "")
                Value = null;
            else
                Value = Convert.ToDateTime(args.Value.ToString());

            OnChange.InvokeAsync(args);

        }

        protected void DatePicker_Enter(FocusEventArgs args)
        {
            //JSRuntime.InvokeVoidAsync("ElementReference.SetSelectionRange", Element, 0, 2);

            OnEnter.InvokeAsync(args);

        }

        protected void DatePicker_Leave(FocusEventArgs args)
        {
            OnLeave.InvokeAsync(args);
        }

    }

}