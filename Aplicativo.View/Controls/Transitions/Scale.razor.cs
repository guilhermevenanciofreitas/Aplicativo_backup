using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicativo.View.Controls.Transitions
{
    public class ScaleTransition : HelpComponent
    {

        [Parameter] public string Class { get; set; }

        [Parameter] public bool In { get; set; } = true;

        [Parameter] public RenderFragment ChildContent { get; set; }

    }
}
