using Aplicativo.Server;
using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Build.Server.Controllers
{
    public class _ControllerBase : ControllerBase
    {

        protected Response CommandQuery(HelpQuery Query)
        {

            var Response = new Response();

            using var db = new Context();

            var query = AsQueryable(db, Query.Table);

            foreach (var item in Query.Where)
            {
                query = query.Where(item.Predicate, item.Args);
            }

            foreach (var item in Query.Includes)
            {
                query = query.Include(item);
            }

            if (Query.Take != null)
            {
                query = query.Take((int)Query.Take);
            }

            Response.Data = query.AsQueryable<object>().ToList();

            return Response;

        }


        protected void Update<T>(Context db, List<T> List, bool UpdateIncludes) where T : class
        {
            var Objects = new List<object>();

            foreach(var item in List)
            {
                Objects.Add(item);
            }

            Update(db, Objects, UpdateIncludes);

        }

        protected void Update(Context db, List<object> List, bool UpdateIncludes)
        {

            foreach (var item in List)
            {

                var PrimaryKey = db.Model.FindEntityType(item.GetType()).FindPrimaryKey().Properties.First().Name;

                if (item.GetType().GetProperty(PrimaryKey).GetValue(item) == null)
                {
                    db.Add(item);
                }
                else
                {

                    if (UpdateIncludes) RemoveInclude(db, item);

                    db.Update(item);
                }
            }

        }


        protected void RemoveInclude(Context db, object item)
        {

            var Collections = item.GetType().GetProperties().Where(c => c.PropertyType.IsGenericType && c.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>));

            foreach (var item2 in Collections)
            {
                Remove<int?>(db, item, item2.PropertyType.GetGenericArguments().Single(), item2.Name);
            }

        }

        protected void Remove<TypePrimaryKey>(Context db, object item, Type type, string name)
        {

            var TablePrimaryKey = db.Model.FindEntityType(item.GetType()).FindPrimaryKey().Properties.First().Name;

            var PrimaryKey = db.Model.FindEntityType(type).FindPrimaryKey().Properties.First().Name;

            var List = ((IEnumerable)item.GetType().GetProperty(name).GetValue(item)).Cast<object>().ToList().Select(c => c.GetType().GetProperty(PrimaryKey).GetValue(c));

            if (List != null)
            {
                var Remove = AsQueryable(db, type.Name).Where(TablePrimaryKey + " == @0 && !@1.Contains(" + PrimaryKey + ")", item.GetType().GetProperty(TablePrimaryKey).GetValue(item), ((IEnumerable)List).Cast<TypePrimaryKey>().ToList());
                db.RemoveRange(Remove);
            }

        }

        protected static IQueryable<object> AsQueryable(Context _context, string type)
        {
            var query = _context.GetType().GetProperty(type).GetValue(_context);
            return (IQueryable<object>)query.GetType().GetMethod("AsQueryable").Invoke(query, null);
        }

        protected Response Exception(Exception ex, Response Response)
        {
            Response.StatusCode = Aplicativo.Utils.StatusCode.Error;
            Response.Data = ex.Message;
            return Response;
        }

    }
}
