using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Aplicativo.View.Controls
{

    public class ITab
    {
        public string Id { get; set; }
        public bool Visible { get; set; }
        public RenderFragment ChildContent { get; set; }
    }

    public class TabSetControl : HelpComponent
    {

        public string Id { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback Changed { get; set; }

        public List<ITab> Tabs { get; set; } = new List<ITab>();

        public void AddTab(ITab Tab)
        {

            Tabs.Add(Tab);

            if (!Tabs.Any(c => c.Visible))
            {
                Active(Tab.Id);
            }

        }

        public void RemoveTab(ITab Tab)
        {

            Tabs.Remove(Tab);

            if (Tab.Visible == true)
            {
                Active(Tabs.LastOrDefault()?.Id);
            }

        }

        public async void Active(string Id)
        {

            foreach(var Item in Tabs)
            {
                Item.Visible = false;
            }

            var Tab = Tabs.FirstOrDefault(c => c.Id == Id);

            Tab.Visible = true;

            this.Id = Id;

            await Changed.InvokeAsync(null);
            StateHasChanged();

        }
    }
}