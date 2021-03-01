using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace Build.Server.Helpers
{

    public static class HelpQuery
    {

        public static IQueryable<T> Filtro<T>(IQueryable<T> Query, Request Request)
        {

            var Filtro = JsonConvert.DeserializeObject<List<HelpFiltro>>(Request.GetParameter("Filtro"));

            foreach (var item in Filtro)
            {

                switch (item.Type)
                {
                    case FiltroType.TextBox:

                        if (item?.Search?[0] != null)
                        {
                            Query = Query.Where(item.Column + ".Contains(@0)", item.Search);
                        }

                        break;
                }

            }


            return Query;

        }

    }
}
