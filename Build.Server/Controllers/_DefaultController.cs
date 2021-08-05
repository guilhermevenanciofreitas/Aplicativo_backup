using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Build.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DefaultController : _ControllerBase
    {
       
        [HttpPost]
        [Route("[action]")]
        public Response Query([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                //var Database = Request.GetParameter("Database");

                var Query = JsonConvert.DeserializeObject<HelpQuery<object>>(Request.GetParameter("Query"));

                return CommandQuery(Query.Database, Query);

            }
            catch (Exception ex)
            {
                return _Exception.Response(ex, Response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public Response Update([FromBody] Request Request)
        {
            var Response = new Response();

            try
            {

                var Database = Request.GetParameter("Database");

                var db = new Context(Database);

                var List = new List<Update>();

                foreach(var Item in JsonConvert.DeserializeObject<List<Update>>(Request.GetParameter("Update")))
                {

                    var Type = db.Model.FindEntityType("Aplicativo.Utils.Models." + Item.Table).ClrType;

                    var Object = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(Item.Object), Type);

                    Item.Object = Object;

                    List.Add(Item);

                }

                Update(db, List);

                db.SaveChanges();

                Response.Data = List.Select(c => c.Object).ToList();

                return Response;

            }
            catch (Exception ex)
            {
                return _Exception.Response(ex, Response);
            }
        }

        //private void SetNull(object Obj)
        //{

        //    var Virtuals = Obj.GetType().GetProperties().Where(c => c.GetGetMethod()?.IsVirtual ?? false && (c.PropertyType.IsGenericType && c.PropertyType.GetType() != typeof(ICollection<>))).ToList();

        //    foreach (var Item in Virtuals)
        //    {
        //        Obj.GetType().GetProperty(Item.Name).SetValue(Obj, null);
        //    }

        //    var Collections = Obj.GetType().GetProperties().Where(c => c.PropertyType.IsGenericType && c.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)).ToList(); //Obj.GetType().GetProperties().Where(c => c.PropertyType.IsGenericType && c.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)).ToList();

        //    foreach (var Item in Collections)
        //    {
        //        foreach(var Item2 in ((IEnumerable)Obj.GetType().GetProperty(Item.Name).GetValue(Obj)).Cast<object>().ToList())
        //        {
        //            SetNull(Item2);
        //        }
        //    }

        //}

    }
}