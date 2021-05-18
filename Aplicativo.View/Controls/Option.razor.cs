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
    public class OptionControl : ComponentBase
    {

        [CascadingParameter] public Options ContainerOptions { get; set; }

        [Parameter] public string Label { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        protected string Name => ContainerOptions?.Name;

        protected bool Checked => ContainerOptions.Options.FirstOrDefault(c => c.Value == Value).Checked;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ContainerOptions.AddOption(new IOption()
            {
                Value = Value,
                Checked = ContainerOptions.Value == Value,
            });
        }

        protected async Task Radio_Change(ChangeEventArgs args)
        {
            ContainerOptions.Value = args.Value.ToString();
            await ContainerOptions.Change(ContainerOptions.Value);
        }

    }
}
