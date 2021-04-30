using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{
    public class TabPageControl : HelpComponent
    {

        [CascadingParameter] public TabSet ContainerTabSet { get; set; }

        [Parameter] public string Id { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        protected bool Visible => ContainerTabSet.Tabs.FirstOrDefault(c => c.Id == Id)?.Visible == true ? true : false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ContainerTabSet.AddTab(new ITab()
            {
                Id = Id,
                Visible = false,
                ChildContent = ChildContent,
            });
        }

        protected async Task Active()
        {
            await ContainerTabSet.Active(Id);
        }

    }
}
