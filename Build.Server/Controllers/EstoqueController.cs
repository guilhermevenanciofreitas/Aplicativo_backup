using Aplicativo.Server;
using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
       
        [HttpPost]
        [Route("[action]")]
        public Response GetEstoqueMovimentoItem([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {
                using var db = new Context();



                var Query = db.EstoqueMovimentoItem
                    .Include(c => c.Produto)
                    .Include(c => c.EstoqueMovimentoItemEntrada)
                    .Include(c => c.EstoqueMovimentoItemSaida)
                    .AsQueryable();

                var Where = JsonConvert.DeserializeObject<List<Where>>(Request.GetParameter("Where"));

                foreach (var item in Where)
                {
                    //Query = Query.Where(item.Predicate, item.Args);
                }

                //Query = HelpQuery.Filtro(Query, Request);

                Response.Data = Query.FirstOrDefault();

                return Response;

            }
            catch (Exception ex)
            {
                Response.StatusCode = Aplicativo.Utils.StatusCode.Error;
                Response.Data = ex.Message;
                return Response;
            }

        }

    }
}