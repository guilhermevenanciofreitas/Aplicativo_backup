using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{

    public enum CharacterCasing
    {
        LowerCase,
        None,
        UpperCase
    }

    public class TextBoxComponent : HelpComponent
    {

        protected ElementReference TextBox;

        [Parameter] public string _Label { get; set; }
        [Parameter] public string _Text { get; set; }
        [Parameter] public string _PlaceHolder { get; set; }
        [Parameter] public bool _ReadOnly { get; set; }
        [Parameter] public CharacterCasing _CharacterCasing { get; set; } = CharacterCasing.UpperCase;

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

                    Text = _Text;
                    PlaceHolder = _PlaceHolder;

                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                }
            }
        }

        public void Focus()
        {
            //TextBox.Focus(JSRuntime);
        }

    }
}