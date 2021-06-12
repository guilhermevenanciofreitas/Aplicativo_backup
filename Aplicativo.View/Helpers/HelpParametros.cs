using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
//using Aplicativo.View.Utils;
//using Blazored.SessionStorage;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{

    public class HelpParametros
    {


        public static Parametros Parametros = new Parametros();

        public static async Task CarregarParametros(int? UsuarioID, int? EmpresaID)
        {

            var QueryUsuario = new HelpQuery<Usuario>();
            QueryUsuario.AddWhere("UsuarioID == @0", UsuarioID);

            var QueryEmpresa = new HelpQuery<Empresa>();
            QueryEmpresa.AddWhere("EmpresaID == @0", EmpresaID);

            var QueryEstado = new HelpQuery<Estado>();
            var QueryUnidadeMedida = new HelpQuery<UnidadeMedida>();



            Parametros.UsuarioLogado = await QueryUsuario.FirstOrDefault();
            Parametros.EmpresaLogada = await QueryEmpresa.FirstOrDefault();

            Parametros.Estado = await QueryEstado.ToList();
            Parametros.UnidadeMedida = await QueryUnidadeMedida.ToList();

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

        public static async Task<bool> VerificarUsuarioLogado()
        {
            try
            {

                HelpConexao.Dominio = await HelpConexao.GetDominio();

                if (string.IsNullOrEmpty(HelpConexao.Dominio.Name))
                {
                    return false;
                }

                if (Parametros.UsuarioLogado != null)
                {
                    return true;
                }

                var ManterConectado = await HelpCookie.Get("ManterConectado");


                if (!string.IsNullOrEmpty(ManterConectado))
                {

                    var Dados = ManterConectado.Split('§');

                    var UsuarioID = Dados[0].ToInt();
                    var EmpresaID = Dados[1].ToInt();

                    await CarregarParametros(UsuarioID, EmpresaID);

                    return true;

                }

                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class Parametros
    {

        public Usuario UsuarioLogado { get; set; }

        public Empresa EmpresaLogada { get; set; }



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
