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
using System.Security.Cryptography.X509Certificates;
using UAParser;

namespace Sistema.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
       
        [HttpPost]
        [Route("[action]")]
        public Response InfoCertificate([FromBody] Request Request)
        {

            var Response = new Response();

            try
            {

                var Certificate = Request.GetParameter("Certificate").ToStringOrNull();
                var Password = Request.GetParameter("Password").ToStringOrNull();

                var cert = new X509Certificate2(Convert.FromBase64String(Certificate), Password);

                var Certificado = new Certificado();


                Certificado.Nome = cert.Subject;
                Certificado.Serial = cert.GetSerialNumberString();
                Certificado.Expira = Convert.ToDateTime(cert.GetExpirationDateString());


                Response.Data = Certificado;

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