using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
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

        protected Response CommandQuery(string Database, HelpQuery<object> Query)
        {

            var Response = new Response();


            using var db = new Context(Database);

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

            Response.Data = query.AsQueryable().ToList();

            return Response;

        }

        //protected void Update<T>(Context db, List<T> List, bool UpdateIncludes) where T : class
        //{
        //    var Objects = new List<object>();

        //    foreach(var item in List)
        //    {
        //        Objects.Add(item);
        //    }

        //    Update(db, Objects, UpdateIncludes);

        //}

        protected void Update(Context db, List<Update> List)
        {

            foreach (var item in List)
            {

                var PrimaryKey = db.Model.FindEntityType(item.Object.GetType()).FindPrimaryKey().Properties.First().Name;

                if (item.Object.GetType().GetProperty(PrimaryKey).GetValue(item.Object) == null)
                {
                    db.Add(item.Object);
                }
                else
                {

                    //if (item.RemoveIncludes)
                    //{
                        RemoveInclude(db, item.Object);
                    //}
                    
                    db.Update(item.Object);

                }
            }

        }


        protected void RemoveInclude(Context db, object Item)
        {

            var Collections = Item.GetType().GetProperties().Where(c => c.PropertyType.IsGenericType && c.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>));

            foreach (var Collection in Collections)
            {
                if (Item != null)
                {
                    Remove<int?>(db, Item, Collection.PropertyType.GetGenericArguments().Single(), Collection.Name);
                }
            }

        }

        protected void Remove<TypePrimaryKey>(Context db, object Item, Type Type, string Name)
        {

            var TablePrimaryKey = db.Model.FindEntityType(Item.GetType()).FindPrimaryKey().Properties.First().Name;

            var PrimaryKey = db.Model.FindEntityType(Type).FindPrimaryKey().Properties.First().Name;

            var Obj = ((IEnumerable)Item.GetType().GetProperty(Name).GetValue(Item));

            if (Obj != null)
            {

                var List = Obj.Cast<object>().ToList().Select(c => c.GetType().GetProperty(PrimaryKey).GetValue(c));

                if (List != null)
                {
                    var Remove = AsQueryable(db, Type.Name).Where(TablePrimaryKey + " == @0 && !@1.Contains(" + PrimaryKey + ")", Item.GetType().GetProperty(TablePrimaryKey).GetValue(Item), ((IEnumerable)List).Cast<TypePrimaryKey>().ToList());
                    db.RemoveRange(Remove);
                }

            }

        }

        protected static IQueryable<object> AsQueryable(Context _context, string type)
        {
            var query = _context.GetType().GetProperty(type).GetValue(_context);
            return (IQueryable<object>)query.GetType().GetMethod("AsQueryable").Invoke(query, null);
        }

    }
}
