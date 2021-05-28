using Aplicativo.Server;
using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Build.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
                return Exception(ex, Response);
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

                var Table = Request.GetParameter("Table").ToString();

                var db = new Context(Database);

                var Type = db.Model.FindEntityType("Aplicativo.Utils.Models." + Table).ClrType;

                var RemoveIncludes = Request.GetParameter("RemoveIncludes").ToBoolean();

                var List = new List<object>();

                foreach(var Item in JsonConvert.DeserializeObject<List<object>>(Request.GetParameter(Table)))
                {

                    var Obj = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(Item), Type);

                    var Virtuals = Obj.GetType().GetProperties().Where(c => c.GetGetMethod().IsVirtual && (!c.PropertyType.IsGenericType && c.PropertyType.GetType() != typeof(ICollection<>))).ToList();

                    foreach(var Item2 in Virtuals)
                    {
                        Obj.GetType().GetProperty(Item2.Name).SetValue(Obj, null);
                    }

                    List.Add(Obj);
                }

                Update(db, List, RemoveIncludes);

                db.SaveChanges();

                Response.Data = List;

                return Response;

            }
            catch (Exception ex)
            {
                return Exception(ex, Response);
            }
        }

        //[HttpPost]
        //[Route("[action]")]
        //public Response Delete([FromBody] Request Request)
        //{

        //    var Response = new Response();

        //    try
        //    {

        //        var Database = Request.GetParameter("Database");

        //        var db = new Context(Database);

        //        var Table = Request.GetParameter("Table").ToString();

        //        var List = new List<object>();

        //        var Type = db.Model.FindEntityType("Aplicativo.Utils.Models." + Table).ClrType;

        //        foreach (var item in JsonConvert.DeserializeObject<List<object>>(Request.GetParameter(Table)))
        //        {
        //            List.Add(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item), Type));
        //        }


        //        foreach(var item in List)
        //        {
        //            item.GetType().GetProperty("Ativo").SetValue(item, false);
        //            db.Update(item);
        //        }

        //        db.SaveChanges();

        //        return Response;

        //    }
        //    catch (Exception ex)
        //    {
        //        return Exception(ex, Response);
        //    }
        //}
    }
}