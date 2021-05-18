using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{

    public class IOption
    {
        public string Value { get; set; }
        public bool Checked { get; set; }
    }

    public class OptionsControl : ComponentBase
    {

        [Parameter] public string _Name { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback OnChange { get; set; }

        public List<IOption> Options { get; set; } = new List<IOption>();

        private string _Value { get; set; }

        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                
                foreach(var item in Options)
                {
                    item.Checked = false;
                }

                var Option = Options.FirstOrDefault(c => c.Value == value);

                Option.Checked = true;

                _Value = value;

                StateHasChanged();

            }
        }

        public async Task Change(string Value)
        {
            await OnChange.InvokeAsync(Value);
        }

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                StateHasChanged();
            }
        }

        public void AddOption(IOption Option)
        {
            Options.Add(Option);
        }

    }
}
