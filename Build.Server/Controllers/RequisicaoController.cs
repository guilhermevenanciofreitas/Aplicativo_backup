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

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RequisicaoController : ControllerBase
    {
       
        [HttpPost]
        [Route("[action]")]
        public Response GetAll([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                using var db = new Context();

                var Query = db.Requisicao.AsQueryable();

                //Query = Query.Where(c => c.Ativo == true);

                Query = HelpQuery.Filtro(Query, Request);

                Response.Data = Query.ToList();

                return Response;

            }
            catch (Exception ex)
            {
                Response.StatusCode = Aplicativo.Utils.StatusCode.Error;
                Response.Data = ex.Message;
                return Response;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public Response Get([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                using var db = new Context();

                var query = db.Requisicao
                    .Include(c => c.RequisicaoItem).ThenInclude(c => c.Produto)
                    .AsQueryable();

                var RequisicaoID = Request.GetParameter("RequisicaoID").ToIntOrNull();

                Response.Data = query.FirstOrDefault(c => c.RequisicaoID == RequisicaoID);

                return Response;

            }
            catch (Exception ex)
            {
                Response.StatusCode = Aplicativo.Utils.StatusCode.Error;
                Response.Data = ex.Message;
                return Response;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public Response Save([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                using var db = new Context();

                var Requisicoes = JsonConvert.DeserializeObject<List<Requisicao>>(Request.GetParameter("Requisicoes"));

                foreach (var item in Requisicoes)
                {

                    //Limpar relacionamentos
                    foreach(var item2 in item.RequisicaoItem)
                    {
                        item2.Requisicao = null;
                        item2.Produto = null;
                    }


                    //Somente saída em itens que ainda não houve saída
                    foreach (var item2 in item.RequisicaoItem.Where(c => c.DataSaida == null))
                    {

                        var EstoqueMovimentoItemEntrada = db.EstoqueMovimentoItemEntrada.FirstOrDefault(c => c.EstoqueMovimentoItemEntradaID == item2.EstoqueMovimentoItemEntradaID);

                        EstoqueMovimentoItemEntrada.Saldo -= item2.Quantidade;

                        db.EstoqueMovimentoItemEntrada.Update(EstoqueMovimentoItemEntrada);

                        item2.DataSaida = DateTime.Now;

                    }


                    if (item.RequisicaoID == null)
                    {
                        db.Requisicao.Add(item);
                    }
                    else
                    {

                        var RequisicaoItem = db.RequisicaoItem.Where(c => c.RequisicaoID == item.RequisicaoID && !item.RequisicaoItem.Select(c => c.RequisicaoItemID).Contains(c.RequisicaoItemID)).ToList();

                        foreach(var item2 in RequisicaoItem.Where(c => c.DataSaida != null))
                        {
                            var EstoqueMovimentoItemEntrada = db.EstoqueMovimentoItemEntrada.FirstOrDefault(c => c.EstoqueMovimentoItemEntradaID == item2.EstoqueMovimentoItemEntradaID);

                            EstoqueMovimentoItemEntrada.Saldo += item2.Quantidade;

                            db.EstoqueMovimentoItemEntrada.Update(EstoqueMovimentoItemEntrada);

                        }

                        db.RequisicaoItem.RemoveRange(RequisicaoItem);

                        db.Requisicao.Update(item);
                    }
                }

                db.SaveChanges();

                Response.Data = Requisicoes;

                return Response;

            }
            catch (Exception ex)
            {
                Response.StatusCode = Aplicativo.Utils.StatusCode.Error;
                Response.Data = ex.Message;
                return Response;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public Response Finalizar([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                using var db = new Context();

                var Requisicoes = JsonConvert.DeserializeObject<List<Requisicao>>(Request.GetParameter("Requisicoes"));


                foreach (var item in Requisicoes)
                {

                    item.DataEntrada = DateTime.Now;

                    foreach (var item2 in item.RequisicaoItem)
                    {
                        var EstoqueMovimentoItemEntrada = db.EstoqueMovimentoItemEntrada.FirstOrDefault(c => c.EstoqueMovimentoItemEntradaID == item2.EstoqueMovimentoItemEntradaID);

                        EstoqueMovimentoItemEntrada.Saldo += item2.Quantidade;

                        db.EstoqueMovimentoItemEntrada.Update(EstoqueMovimentoItemEntrada);

                        item2.DataEntrada = DateTime.Now;

                    }

                    db.Requisicao.Update(item);
                }

                db.SaveChanges();

                Response.Data = Requisicoes;

                return Response;

            }
            catch (Exception ex)
            {
                Response.StatusCode = Aplicativo.Utils.StatusCode.Error;
                Response.Data = ex.Message;
                return Response;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public Response Delete([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                using var db = new Context();

                var Requisicoes = JsonConvert.DeserializeObject<List<int?>>(Request.GetParameter("Requisicoes"));

                var Items = db.Requisicao.Where(c => Requisicoes.Contains(c.RequisicaoID)).ToList();

                foreach (var item in Items)
                {
                    //item.Ativo = false;
                    db.Requisicao.Update(item);
                }

                db.SaveChanges();

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