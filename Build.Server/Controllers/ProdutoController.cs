using Aplicativo.Server;
using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Build.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using UAParser;

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
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

            var Query = db.Produto.AsQueryable();

            Query = Query.Where(c => c.Ativo == true);

            Query = HelpQuery.Filtro(Query, Request);

            Response.Data = Query.ToList();

            return Response;

        }

        [HttpPost]
        [Route("[action]")]
        public Response Get([FromBody] Request Request)
        {

            using var db = new Context();

            var Response = new Response();

            var query = db.Produto
                .Include(c => c.ProdutoFornecedor).ThenInclude(c => c.Fornecedor)
                .Include(c => c.ProdutoFornecedor).ThenInclude(c => c.UnidadeMedida)
                .AsQueryable();

            query = query.Where(c => c.Ativo == true);

            var ProdutoID = Request.GetParameter("ProdutoID").ToIntOrNull();

            Response.Data = query.FirstOrDefault(c => c.ProdutoID == ProdutoID);

            return Response;

        }

        [HttpPost]
        [Route("[action]")]
        public Response Save([FromBody] Request Request)
        {

            using var db = new Context();

            var Response = new Response();

            var Produtos = JsonConvert.DeserializeObject<List<Produto>>(Request.GetParameter("Produtos"));

            foreach(var item in Produtos)
            {
                if (item.ProdutoID == null)
                {
                    db.Produto.Add(item);
                }
                else
                {

                    //var UsuarioEmail = db.UsuarioEmail.Where(c => c.UsuarioID == item.UsuarioID && !item.UsuarioEmail.Select(c => c.UsuarioEmailID).Contains(c.UsuarioEmailID)).ToList();
                    //db.UsuarioEmail.RemoveRange(UsuarioEmail);
                    
                    db.Produto.Update(item);

                }
            }

            db.SaveChanges();


            foreach (var item in Produtos)
            {
                if (item.Codigo == null)
                {
                    item.Codigo = item.ProdutoID.ToStringOrNull();
                    db.Produto.Update(item);
                }
            }

            db.SaveChanges();

            Response.Data = Produtos;

            return Response;

        }

        [HttpPost]
        [Route("[action]")]
        public Response Delete([FromBody] Request Request)
        {

            using var db = new Context();

            var Response = new Response();

            var Produtos = JsonConvert.DeserializeObject<List<int?>>(Request.GetParameter("Produtos"));

            var Items = db.Produto.Where(c => Produtos.Contains(c.ProdutoID)).ToList();

            foreach(var item in Items)
            {
                item.Ativo = false;
                db.Produto.Update(item);
            }

            db.SaveChanges();

            return Response;

        }

    }
}