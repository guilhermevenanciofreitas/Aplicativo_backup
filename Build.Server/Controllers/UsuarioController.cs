using Aplicativo.Server;
using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UAParser;

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
       
        [HttpPost]
        [Route("[action]")]
        public Response GetAll([FromBody] Request Request)
        {

            //var IP = HttpContext.Connection.RemoteIpAddress.ToString();

            //var userAgent = HttpContext.Request.Headers["User-Agent"];

            //var uaParser = Parser.GetDefault();
            //ClientInfo ClientInfo = uaParser.Parse(userAgent);



            using var db = new Context();

            var Response = new Response();

            var query = db.Usuario.AsQueryable();

            query = query.Where(c => c.Ativo == true);

            Response.Data = query.ToList();

            return Response;

        }

        [HttpPost]
        [Route("[action]")]
        public Response Get([FromBody] Request Request)
        {

            using var db = new Context();

            var Response = new Response();

            var query = db.Usuario.Include(c => c.UsuarioEmail).AsQueryable();

            query = query.Where(c => c.Ativo == true);

            var UsuarioID = Request.GetParameter("UsuarioID").ToIntOrNull();

            Response.Data = query.FirstOrDefault(c => c.UsuarioID == UsuarioID);

            return Response;

        }

        [HttpPost]
        [Route("[action]")]
        public Response Save([FromBody] Request Request)
        {

            using var db = new Context();

            var Response = new Response();

            var Usuarios = JsonConvert.DeserializeObject<List<Usuario>>(Request.GetParameter("Usuarios"));

            foreach(var item in Usuarios)
            {
                if (item.UsuarioID == null)
                {
                    db.Usuario.Add(item);
                }
                else
                {
                    db.Usuario.Update(item);
                }
            }

            db.SaveChanges();

            Response.Data = Usuarios;

            return Response;

        }

        [HttpPost]
        [Route("[action]")]
        public Response Delete([FromBody] Request Request)
        {

            using var db = new Context();

            var Response = new Response();

            var Usuarios = JsonConvert.DeserializeObject<List<int?>>(Request.GetParameter("Usuarios"));

            var Items = db.Usuario.Where(c => Usuarios.Contains(c.UsuarioID)).ToList();

            foreach(var item in Items)
            {
                item.Ativo = false;
                db.Usuario.Update(item);
            }

            db.SaveChanges();

            return Response;

        }

    }
}