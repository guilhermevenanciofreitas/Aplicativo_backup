using Aplicativo.Utils.Helpers;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{
    public partial class ViewPesquisaModalControl<Tipo> : ComponentBase
    {

        public List<Where> Where { get; set; } = new List<Where>();

        public int? Take { get; set; }

        public List<Column> Columns { get; set; } = new List<Column>();

        public DropDownList DplCampo { get; set; }
        public TextBox TxtPesquisa { get; set; }

        public SfGrid<Tipo> GridView { get; set; }
        public List<Tipo> ListView { get; set; } = new List<Tipo>();

        [Parameter] public EventCallback OnClick { get; set; }

        protected void DplCampo_Change()
        {
            TxtPesquisa.Focus();
        }

        public async Task TxtPesquisa_Input()
        {

            var Column = Columns?.FirstOrDefault(c => c.Field == DplCampo.SelectedValue);

            var Query = new HelpQuery<Tipo>();

            foreach(var where in Where)
            {
                Query.AddWhere(where.Predicate, where.Args);
            }

            if (Take != null)
            {
                Query.AddTake(Take);
            }

            if (!string.IsNullOrEmpty(TxtPesquisa.Text))
            {

                if (Column.Type == typeof(short) || Column.Type == typeof(int) || Column.Type == typeof(long))
                {
                    Query.AddWhere(Column.Field + " == @0", TxtPesquisa.Text);
                }
                if (Column.Type == typeof(string))
                {
                    Query.AddWhere(Column.Field + ".Contains(@0)", TxtPesquisa.Text);
                }

            }

            var Items = await Query.ToList();

            ListView.Clear();
            //ListView = new List<Tipo>();
            ListView.AddRange(Items);

            GridView.Refresh();

        }

        protected void GridView_RowClick(RecordClickEventArgs<Tipo> args)
        {
            OnClick.InvokeAsync(args.RowData);
        }

        public void Refresh()
        {

            DplCampo.Items.Clear();

            foreach (var item in Columns)
            {
                DplCampo.Add(item.Field, item.HeaderText);
            }

            StateHasChanged();

        }

    }

    public class Column
    {
        public string Field { get; set; }
        public string HeaderText { get; set; }
        public Type Type { get; set; }
        public string FieldFormat { get; set; }
        

        public Column(string Field, string HeaderText, Type Type, string FieldFormat = null)
        {
            this.Field = Field;
            this.HeaderText = HeaderText;
            this.Type = Type;
            this.FieldFormat = FieldFormat;
        }
    }
}
