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
    public enum TextBoxType
    {
        String,
        Password,
    }

    public enum CharacterCasing
    {
        LowerCase,
        None,
        UpperCase
    }

    public class TextBoxComponent : ComponentBase
    {

        protected TextField TextField;

        public ElementReference Element;

        [Parameter] public TextBoxType _Type { get; set; } = TextBoxType.String;
        [Parameter] public string _Label { get; set; }
        [Parameter] public string _Text { get; set; }
        [Parameter] public string _Mask { get; set; }
        [Parameter] public string _PlaceHolder { get; set; }
        [Parameter] public bool _ReadOnly { get; set; }
        [Parameter] public int? _TabIndex { get; set; }
        [Parameter] public CharacterCasing _CharacterCasing { get; set; } = CharacterCasing.UpperCase;

        [Parameter] public EventCallback OnInput { get; set; }
        [Parameter] public EventCallback OnKeyUp { get; set; }
        [Parameter] public EventCallback OnLeave { get; set; }

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

        public string Text
        {
            get
            {
                switch (_CharacterCasing)
                {
                    case CharacterCasing.LowerCase:
                        return _Text?.ToLower();
                    case CharacterCasing.UpperCase:
                        return _Text?.ToUpper();
                    default:
                        return _Text;
                }
            }
            set
            {
                switch (_CharacterCasing)
                {
                    case CharacterCasing.LowerCase:
                        _Text = value?.ToLower();
                        break;
                    case CharacterCasing.UpperCase:
                        _Text = value?.ToUpper();
                        break;
                    default:
                        _Text = value;
                        break;
                }
                StateHasChanged();
            }
        }

        public string Mask
        {
            get => _Mask;
            set 
            {
                _Mask = value;
                App.JSRuntime.InvokeVoidAsync("ElementReference.Mask", Element, _Mask);
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

        //public void SetMask(string Mask)
        //{
        //    App.JSRuntime.InvokeVoidAsync("ElementReference.Mask", Element, Mask);
        //}


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {

                    Text = _Text;
                    PlaceHolder = _PlaceHolder;

                    if (!string.IsNullOrEmpty(_Mask))
                    {

                        //if (HelpParametros.Template == Template.Mobile)
                        //{
                        //    await App.JSRuntime.InvokeVoidAsync("ElementReference.Mask", TextField.RootRef.Current.Value, _Mask);
                        //}
                        //else
                        //{
                        //    await App.JSRuntime.InvokeVoidAsync("ElementReference.Mask", Element, _Mask);
                        //}

                        Mask = _Mask;

                    }

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

        protected async Task TextBox_Input(ChangeEventArgs args)
        {
            try
            {
                Text = args.Value.ToString();
                await OnInput.InvokeAsync(args);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task TextBox_KeyUp(KeyboardEventArgs args)
        {
            try
            {
                await OnKeyUp.InvokeAsync(args);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task TextBox_Leave()
        {
            try
            {
                await OnLeave.InvokeAsync(null);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }


    }
}