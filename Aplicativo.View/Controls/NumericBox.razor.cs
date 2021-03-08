using Aplicativo.Utils.Helpers;
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
        [Parameter] public decimal _Text { get; set; }
        [Parameter] public string _PlaceHolder { get; set; }
        [Parameter] public string _Prefix { get; set; } = "";
        [Parameter] public int _Digits { get; set; } = 2;
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

        public async Task<decimal> GetValue()
        {
            try
            {
                return (await JSRuntime.InvokeAsync<string>("ElementReference.GetValue", Element)).ToDecimal();
            }
            catch (Exception)
            {
                return 0M;
            }
        }

        public async Task SetValue(decimal Value)
        {
            await JSRuntime.InvokeVoidAsync("ElementReference.SetValue", Element, Value);
        }

        //public decimal Text
        //{
        //    get
        //    {
        //        return _Text;
        //    }
        //    set
        //    {
        //        _Text = value;
        //        StateHasChanged();
        //    }
        //}



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

                    //Text = _Text;
                    PlaceHolder = _PlaceHolder;

                    await JSRuntime.InvokeVoidAsync("ElementReference.MaskNumber", Element, _Prefix, _Digits);

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