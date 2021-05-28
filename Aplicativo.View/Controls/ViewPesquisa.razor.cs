using Aplicativo.Utils.Helpers;
using Microsoft.AspNetCore.Components;

namespace Aplicativo.View.Controls
{
    public partial class ViewPesquisaControl : ComponentBase
    {

        [Parameter] public string _Label { get; set; }

        public ElementReference Element => TxtCodigo.Element;

        public string Value 
        {
            get => TxtCodigo.Text.ToStringOrNull();
            set => TxtCodigo.Text = value.ToStringOrNull();
        }
        public string Text
        {
            get => TxtDescricao.Text.ToStringOrNull();
            set => TxtDescricao.Text = value.ToStringOrNull();
        }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }

        public void Clear()
        {
            Value = null;
            Text = null;
        }

    }
}