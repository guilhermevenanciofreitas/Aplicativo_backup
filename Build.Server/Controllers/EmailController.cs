using Aplicativo.Server;
using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Mail;
using UAParser;

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
       
        [HttpPost]
        [Route("[action]")]
        public Response Send([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                var Smtp = Request.GetParameter("Smtp").ToStringOrNull();
                var Porta = Request.GetParameter("Porta").ToInt();
                var Email = Request.GetParameter("Email").ToStringOrNull();
                var Senha = Request.GetParameter("Senha").ToStringOrNull();
                var EnableSsl = Request.GetParameter("EnableSsl").ToBoolean();

                var Para = JsonConvert.DeserializeObject<List<string>>(Request.GetParameter("Para"));
                var Assunto = Request.GetParameter("Assunto").ToStringOrNull();
                var Mensagem = Request.GetParameter("Mensagem").ToStringOrNull();


                var SmtpClient = new SmtpClient(Smtp)
                {
                    Port = Porta,
                    Credentials = new NetworkCredential(Email, Senha),
                    EnableSsl = EnableSsl,
                };

                var MailMessage = new MailMessage
                {
                    From = new MailAddress(Email),
                    Subject = Assunto,
                    Body = Mensagem,
                    IsBodyHtml = true,
                };

                foreach(var item in Para)
                {
                    MailMessage.To.Add(item);
                }
                
                SmtpClient.Send(MailMessage);


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