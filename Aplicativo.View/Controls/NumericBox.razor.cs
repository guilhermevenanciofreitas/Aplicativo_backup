using Aplicativo.Utils.Helpers;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Skclusive.Material.Text;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{

    public class NumericBoxComponent : ComponentBase
    {

        protected TextField TextField;

        public ElementReference Element;

        [Parameter] public string _Label { get; set; }
        [Parameter] public decimal _Value { get; set; }
        [Parameter] public int? _MaxLength { get; set; }
        [Parameter] public string _PlaceHolder { get; set; }
        [Parameter] public string _Prefix { get; set; } = "";
        [Parameter] public int _Digits { get; set; } = 2;
        [Parameter] public bool _ReadOnly { get; set; }

        [Parameter] public EventCallback OnLeave { get; set; }
        [Parameter] public EventCallback OnKeyPress { get; set; }
        [Parameter] public EventCallback OnKeyUp { get; set; }
        [Parameter] public EventCallback OnKeyDown { get; set; }

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

        private async Task<decimal> GetValue()
        {
            try
            {

                var Value = (await App.JSRuntime.InvokeAsync<string>("ElementReference.GetValue", Element));

                if (string.IsNullOrEmpty(_Prefix))
                {
                    return Value.ToDecimalOrNull() ?? 0;
                }
                else
                {
                    return Value.Replace(_Prefix, "").ToDecimalOrNull() ?? 0;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error in Controls/NumericBox.razor.cs in function GetValue.\n" + ex.Message);
            }
        }

        public decimal Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                App.JSRuntime.InvokeVoidAsync("ElementReference.SetValue", Element, value);
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

                    await App.JSRuntime.InvokeVoidAsync("ElementReference.MaskNumber", Element, _Prefix, _Digits);

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

        protected async Task NumericBox_OnLeave(FocusEventArgs args)
        {
            try
            {
                await OnLeave.InvokeAsync(args);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task NumericBox_OnKeyDown(KeyboardEventArgs args)
        {
            try
            {
                await OnKeyDown.InvokeAsync(args);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task NumericBox_OnKeyPress(KeyboardEventArgs args)
        {
            try
            {
                await OnKeyPress.InvokeAsync(args);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task NumericBox_OnKeyUp(KeyboardEventArgs args)
        {
            try
            {
                _Value = await GetValue();
                await OnKeyUp.InvokeAsync(_Value);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

    }
}