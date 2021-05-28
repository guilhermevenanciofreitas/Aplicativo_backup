using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace Aplicativo.Utils.Helpers
{
    public class HelpQuery<T> : IDisposable
    {

        public HelpQuery()
        {
            Table = typeof(T).Name;
        }

        public string Database { get; set; }

        public string Table { get; set; }

        public List<string> Includes { get; set; } = new List<string>();

        public List<Where> Where { get; set; } = new List<Where>();

        public int? Take { get; set; }



        public void AddInclude(string Include)
        {
            Includes.Add(Include);
        }

        public void AddWhere(string Predicate, params object[] Args)
        {

            var Where = new Where();

            Where.Predicate = Predicate;
            Where.Args = Args;

            this.Where.Add(Where);

        }

        public void AddFiltro(List<HelpFiltro> HelpFiltro)
        {
            foreach (var item in HelpFiltro)
            {

                switch (item.Type)
                {
                    case FiltroType.TextBox:
                        if (item?.Search?[0] != null) AddWhere(item.Column + ".Contains(@0)", item.Search);
                        break;
                }

            }
        }

        public void AddTake(int? Take)
        {
            this.Take = Take;
        }

        public void Dispose()
        {
            Dispose();
        }
    }

    public class Where
    {

        public string Predicate { get; set; }
        public object[] Args { get; set; }

    }

}
