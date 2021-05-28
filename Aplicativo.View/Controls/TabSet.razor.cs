using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{

    public class ITab
    {
        public ElementReference Element { get; set; }
        public string Id { get; set; }
        public bool Visible { get; set; }
        public RenderFragment ChildContent { get; set; }
    }

    public class TabSetControl : ComponentBase
    {

        public string Id => _Id;

        private string _Id { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback Changed { get; set; }

        public List<ITab> Tabs { get; set; } = new List<ITab>();

        public async void AddTab(ITab Tab)
        {

            Tabs.Add(Tab);

            if (!Tabs.Any(c => c.Visible))
            {
                await Active(Tab.Id);
            }

            StateHasChanged();

        }

        public async void RemoveTab(ITab Tab)
        {

            Tabs.Remove(Tab);

            if (Tab.Visible == true)
            {
                await Active(Tabs.LastOrDefault()?.Id);
            }

        }

        public async Task Active(string Id)
        {

            _Id = Id;

            Tabs.ForEach(c => c.Visible = false);

            Tabs.Where(c => c.Id == _Id).ToList().ForEach(c => c.Visible = true);

            StateHasChanged();

            await Changed.InvokeAsync(null);

            StateHasChanged();

        }
    }
}