using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{
    public class DropDownListComponent : HelpComponent
    {

        public ElementReference Element;

        [Parameter] public string _Label { get; set; }

        [Parameter] public List<DropDownListItem> _Items { get; set; } = new List<DropDownListItem>();

        [Parameter] public EventCallback Changed { get; set; }

        public string SelectedValue
        {
            get
            {
                return _SelectedValue;
            }
            set
            {
                _SelectedValue = value;
                _Changed(value);
                StateHasChanged();
            }
        }

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

        public List<DropDownListItem> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
                StateHasChanged();
            }
        }

        protected string _SelectedValue { get; set; }

        public int SelectedIndex
        {
            get
            {
                return _Items.IndexOf(_Items?.FirstOrDefault(c => c.Value == SelectedValue));
            }
            set
            {
                SelectedValue = _Items?[value].Value;
                StateHasChanged();
            }
        }

        public string SelectedText
        {
            get
            {
                return _Items.FirstOrDefault(c => c.Value == SelectedValue)?.Text;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                }
            }
        }

        public void Focus()
        {
            Element.Focus(JSRuntime);
        }

        //public void LoadDropDownList<T>(string valueField, string textField, DropDownListItem First = null, Expression<Func<T, bool>> Condition = null, string OrderBy = null) where T : class
        //{

        //    using var db = new Context();
        //    var query = db.Set<T>().AsQueryable();

        //    if (Condition != null)
        //    {
        //        query = query.Where(Condition);
        //    }

        //    if (OrderBy != null)
        //    {
        //        query = query.OrderBy(OrderBy);
        //    }

        //    var Items = query.Select(c => new DropDownListItem()
        //    {
        //        Value = c.GetType().GetProperty(valueField).GetValue(c).ToString(),
        //        Text = c.GetType().GetProperty(textField).GetValue(c).ToString(),
        //    }).ToList();

        //    LoadDropDownList(First, Items);

        //}

        public void LoadDropDownList<T>(string valueField, string textField, DropDownListItem First = null, List<T> List = null) where T : class
        {

            var Items = List.Select(c => new DropDownListItem()
            {
                Value = c.GetType().GetProperty(valueField).GetValue(c).ToString(),
                Text = c.GetType().GetProperty(textField).GetValue(c).ToString(),
            }).ToList();


            if (First != null)
            {
                Items.Insert(0, First);
            }

            _Items = Items;

            StateHasChanged();

        }

        public void LoadDropDownList(DropDownListItem First = null, List<DropDownListItem> List = null)
        {

            if (First != null)
            {
                List.Insert(0, First);
            }

            _Items = List;

            StateHasChanged();

        }

        public void Add(string Value, string Text)
        {
            _Items.Add(new DropDownListItem(Value, Text));
            StateHasChanged();
        }

        public void Insert(int Index, string Value, string Text)
        {
            _Items.Insert(Index, new DropDownListItem(Value, Text));
            StateHasChanged();
        }

        protected void _Changed(string args)
        {
            if (args != string.Empty)
            {
                Changed.InvokeAsync(args);
            }
        }

    }

    public class DropDownListItem
    {

        public string Value { get; set; }
        public string Text { get; set; }

        public DropDownListItem()
        { 
        }

        public DropDownListItem(string Value, string Text)
        {
            this.Value = Value;
            this.Text = Text;
        }

    }
}