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
using System.Linq.Dynamic.Core;

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RequisicaoController : _ControllerBase
    {


        //[HttpPost]
        //[Route("[action]")]
        //public Response Salvar([FromBody] Request Request)
        //{

        //    var Response = new Response();

        //    try
        //    {

        //        var Database = Request.GetParameter("Database");

        //        using var db = new Context(Database);

        //        var Requisicao = JsonConvert.DeserializeObject<List<Requisicao>>(Request.GetParameter("Requisicao"));

        //        Update(db, Requisicao);

        //        foreach (var item in Requisicao)
        //        {

        //            //Realiza a entrada dos itens que foram excluídos
        //            var RequisicaoItemExcluido = db.RequisicaoItem.Where(c => c.RequisicaoID == item.RequisicaoID && !item.RequisicaoItem.Select(c => c.RequisicaoItemID).Contains(c.RequisicaoItemID)).ToList();

        //            foreach (var item2 in RequisicaoItemExcluido.Where(c => c.DataSaida != null))
        //            {
        //                var EstoqueMovimentoItemEntrada = db.EstoqueMovimentoItemEntrada.FirstOrDefault(c => c.EstoqueMovimentoItemEntradaID == item2.EstoqueMovimentoItemEntradaID);

        //                EstoqueMovimentoItemEntrada.Saldo += item2.Quantidade;

        //                db.EstoqueMovimentoItemEntrada.Update(EstoqueMovimentoItemEntrada);
        //            }


        //            //Realiza a saída dos novos itens que foram adicionados
        //            foreach (var item2 in item.RequisicaoItem.Where(c => c.DataSaida == null))
        //            {
        //                var EstoqueMovimentoItemEntrada = db.EstoqueMovimentoItemEntrada.FirstOrDefault(c => c.EstoqueMovimentoItemEntradaID == item2.EstoqueMovimentoItemEntradaID);

        //                EstoqueMovimentoItemEntrada.Saldo -= item2.Quantidade;

        //                db.EstoqueMovimentoItemEntrada.Update(EstoqueMovimentoItemEntrada);
        //                item2.DataSaida = DateTime.Now;

        //            }

        //        }

        //        db.SaveChanges();

        //        Response.Data = Requisicao;

        //        return Response;

        //    }
        //    catch (Exception ex)
        //    {
        //        return Exception(ex, Response);
        //    }
        //}

        //[HttpPost]
        //[Route("[action]")]
        //public Response Finalizar([FromBody] Request Request)
        //{

        //    var Response = new Response();

        //    try
        //    {

        //        var Database = Request.GetParameter("Database");

        //        using var db = new Context(Database);

        //        var Requisicao = JsonConvert.DeserializeObject<List<Requisicao>>(Request.GetParameter("Requisicao"));

        //        Update(db, Requisicao);

        //        foreach (var item in Requisicao)
        //        {

        //            var RequisicaoItem = db.RequisicaoItem.Where(c => c.RequisicaoID == item.RequisicaoID).ToList();

        //            foreach (var item2 in RequisicaoItem)
        //            {
        //                var EstoqueMovimentoItemEntrada = db.EstoqueMovimentoItemEntrada.FirstOrDefault(c => c.EstoqueMovimentoItemEntradaID == item2.EstoqueMovimentoItemEntradaID);

        //                EstoqueMovimentoItemEntrada.Saldo += item2.Quantidade;

        //                db.EstoqueMovimentoItemEntrada.Update(EstoqueMovimentoItemEntrada);

        //                item2.DataEntrada = DateTime.Now;

        //            }

        //        }

        //        db.SaveChanges();

        //        Response.Data = Requisicao;

        //        return Response;

        //    }
        //    catch (Exception ex)
        //    {
        //        return Exception(ex, Response);
        //    }
        //}

    }
}