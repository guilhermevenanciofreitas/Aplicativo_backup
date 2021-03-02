using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Material.Text;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{

    public class NumericBoxComponent : HelpComponent
    {

        protected TextField TextField;

        public ElementReference Element;

        [Parameter] public string _Label { get; set; }
        [Parameter] public decimal _Value { get; set; }
        [Parameter] public int _Decimais { get; set; } = 2;
        [Parameter] public string _PlaceHolder { get; set; }
        [Parameter] public bool _ReadOnly { get; set; }
        
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

        public decimal Value
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

                    await JSRuntime.InvokeVoidAsync("ElementReference.Mask", Element, "#,##0." + _Decimais.ToString().PadRight(_Decimais, '0'));

                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                }
            }
        }

        public void Focus()
        {
            Element.Focus(JSRuntime);
            //StateHasChanged();
        }

    }
}