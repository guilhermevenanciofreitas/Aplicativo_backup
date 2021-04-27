using Aplicativo.Utils;
using Aplicativo.Utils.Models;
//using Aplicativo.View.Utils;
//using Blazored.SessionStorage;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{

    public enum Template
    {
        Desktop,
        Mobile,
    }

    public class HelpParametros
    {

        public static Template Template { get; set; } = Template.Desktop;

        public static Parametros Parametros = new Parametros();

        public static void GetParametros(int UsuarioID, int EmpresaID)
        {

            //var cert = new ConfiguracaoCertificado
            //{
            //    TipoCertificado = TipoCertificado.A1Arquivo,
            //    Arquivo = "C:\\Certificado.pfx",
            //    Senha = "tcl@04058687"
            //};

            //var cfg = new ConfiguracaoServico
            //{
            //    DiretorioSchemas = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Schemas"),
            //    Certificado = cert,
            //    tpAmb = TipoAmbiente.Producao,
            //    cUF = DFe.Classes.Entidades.Estado.GO,
            //};


            //using (var db = new Context())
            //{
            //    return (from c in db.Usuario
            //            where c.UsuarioID == UsuarioID
            //            select new Parametros
            //            {
            //                UsuarioLogado = c,
            //                EmpresaLogada = db.Empresa.FirstOrDefault(x => x.EmpresaID == EmpresaID),
            //                //AgendaEtapa = db.AgendaEtapa.ToList(),
            //                //AgendaTipo = db.AgendaTipo.ToList(),
            //                //ArquivoTipo = db.ArquivoTipo.ToList(),
            //                //OrdemCargaStatus = db.OrdemCargaStatus.ToList(),
            //                //Estado = db.Estado.ToList(),
            //                //EnderecoTipo = db.EnderecoTipo.ToList(),
            //                //ContatoTipo = db.ContatoTipo.ToList(),
            //                //UnidadeMedida = db.UnidadeMedida.Where(c => c.Ativo == true).ToList(),
            //                //CompraStatus = db.CompraStatus.ToList(),
            //                //CompraStatusWorkflow = db.CompraStatusWorkflow.ToList(),
            //                //VendaStatus = db.VendaStatus.ToList(),
            //                //VendaStatusWorkflow = db.VendaStatusWorkflow.ToList(),
            //                //Estoque = db.Estoque.ToList(),
            //                //CfgServico = cfg,
            //            }).FirstOrDefault();
            //}

                
        }

        //public static async Task<bool> VerificarUsuarioLogado(ISessionStorageService Session, IJSRuntime JSRuntime)
        //{

        //    var Parametros = await Session.GetItemAsync<Parametros>("Parametros");

        //    if (Parametros == null)
        //    {

        //        var ManterConectado = await HelpCookie.Get(JSRuntime, "ManterConectado");

        //        if (ManterConectado != "")
        //        {
        //            var Dados = ManterConectado.Split('§');

        //            var UsuarioID = HelpCriptografia.Descriptografar(Dados[0]).ToInt();
        //            var EmpresaID = Dados[1].ToInt();

        //            await Session.SetItemAsync("Parametros", GetParametros(UsuarioID, EmpresaID));

        //            return true;
        //        }

        //        return false;

        //    }

        //    return true;

        //}

    }

    public class Parametros
    {

        public Usuario UsuarioLogado { get; set; }

        public Empresa EmpresaLogada { get; set; }

        //public ConfiguracaoServico CfgServico { get; set; }


        //public List<AgendaTipo> AgendaTipo { get; set; } = new List<AgendaTipo>();

        //public List<AgendaEtapa> AgendaEtapa { get; set; } = new List<AgendaEtapa>();

        //public List<ArquivoTipo> ArquivoTipo { get; set; } = new List<ArquivoTipo>();

        //public List<OrdemCargaStatus> OrdemCargaStatus { get; set; } = new List<OrdemCargaStatus>();

        public List<Estado> Estado { get; set; } = new List<Estado>();

        public List<EnderecoTipo> EnderecoTipo { get; set; } = new List<EnderecoTipo>();

        //public List<ContatoTipo> ContatoTipo { get; set; } = new List<ContatoTipo>();

        public List<UnidadeMedida> UnidadeMedida { get; set; } = new List<UnidadeMedida>();

        //public List<CompraStatus> CompraStatus { get; set; } = new List<CompraStatus>();

        //public List<CompraStatusWorkflow> CompraStatusWorkflow { get; set; } = new List<CompraStatusWorkflow>();

        //public List<VendaStatus> VendaStatus { get; set; } = new List<VendaStatus>();

        //public List<VendaStatusWorkflow> VendaStatusWorkflow { get; set; } = new List<VendaStatusWorkflow>();

        //public List<Estoque> Estoque { get; set; } = new List<Estoque>();

    }

}
