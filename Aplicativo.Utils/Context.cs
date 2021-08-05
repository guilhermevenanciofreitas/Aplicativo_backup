using Aplicativo.Utils.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace Aplicativo.Utils
{
    public class Context : DbContext, IDisposable
    {

        //private string ConnectionString { get; set; } = @"Server=localhost\AtlantaSistemas;Database=Dev;User Id=sa;Password=@Rped94ft";

        public Context()
        {
            //this.ConnectionString = ConnectionString;
            //ChangeTracker.LazyLoadingEnabled = true;
        }

        public Context(string ConnectionString)
        {
            //this.ConnectionString = ConnectionString;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        //public DbSet<Agenda> Agenda { get; set; }

        //public DbSet<AgendaArquivo> AgendaArquivo { get; set; }

        //public DbSet<AgendaEtapa> AgendaEtapa { get; set; }

        public DbSet<AgendaTipo> AgendaTipo { get; set; }

        public DbSet<Arquivo> Arquivo { get; set; }

        public DbSet<ArquivoTipo> ArquivoTipo { get; set; }

        public DbSet<Atributo> Atributo { get; set; }

        public DbSet<CentroCusto> CentroCusto { get; set; }

        public DbSet<Certificado> Certificado { get; set; }

        public DbSet<CEST> CEST { get; set; }

        public DbSet<CFOP> CFOP { get; set; }

        public DbSet<Conferencia> Conferencia { get; set; }

        public DbSet<ConferenciaItem> ConferenciaItem { get; set; }

        //public DbSet<Chamado> Chamado { get; set; }

        //public DbSet<ChamadoAndamento> ChamadoAndamento { get; set; }

        //public DbSet<ChamadoArquivo> ChamadoArquivo { get; set; }

        //public DbSet<Compra> Compra { get; set; }

        //public DbSet<CompraItem> CompraItem { get; set; }

        //public DbSet<CompraStatus> CompraStatus { get; set; }

        //public DbSet<CompraStatusWorkflow> CompraStatusWorkflow { get; set; }

        public DbSet<ContaBancaria> ContaBancaria { get; set; }

        public DbSet<ContaBancariaFechamento> ContaBancariaFechamento { get; set; }

        public DbSet<ContaBancariaFormaPagamento> ContaBancariaFormaPagamento { get; set; }

        public DbSet<Contato> Contato { get; set; }

        public DbSet<ContatoTipo> ContatoTipo { get; set; }

        public DbSet<CSOSN_ICMS> CSOSN_ICMS { get; set; }

        public DbSet<CST_ICMS> CST_ICMS { get; set; }

        public DbSet<CST_IPI> CST_IPI { get; set; }

        public DbSet<CST_PISCOFINS> CST_PISCOFINS { get; set; }

        //public DbSet<Cotacao> Cotacao { get; set; }

        //public DbSet<CotacaoItem> CotacaoItem { get; set; }

        //public DbSet<CTe> CTe { get; set; }

        //public DbSet<CTeNotaFiscal> CTeNotaFiscal { get; set; }

        //public DbSet<DistribuicaoNFe> DistribuicaoNFe { get; set; }

        //public DbSet<DistribuicaoNFeConsulta> DistribuicaoNFeConsulta { get; set; }

        //public DbSet<DistribuicaoNFeLote> DistribuicaoNFeLote { get; set; }

        //public DbSet<DistribuicaoNFeNSU> DistribuicaoNFeNSU { get; set; }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<EmpresaCertificado> EmpresaCertificado { get; set; }

        public DbSet<EmpresaEndereco> EmpresaEndereco { get; set; }

        public DbSet<Endereco> Endereco { get; set; }

        public DbSet<EnderecoTipo> EnderecoTipo { get; set; }

        public DbSet<Estado> Estado { get; set; }

        public DbSet<Estoque> Estoque { get; set; }

        public DbSet<EstoqueMovimento> EstoqueMovimento { get; set; }

        public DbSet<EstoqueMovimentoItem> EstoqueMovimentoItem { get; set; }

        public DbSet<EstoqueMovimentoItemEntrada> EstoqueMovimentoItemEntrada { get; set; }

        public DbSet<EstoqueMovimentoItemSaida> EstoqueMovimentoItemSaida { get; set; }

        public DbSet<FormaPagamento> FormaPagamento { get; set; }

        //public DbSet<Motivo> Motivo { get; set; }

        public DbSet<Municipio> Municipio { get; set; }

        public DbSet<NCM> NCM { get; set; }

        public DbSet<NotaFiscal> NotaFiscal { get; set; }

        public DbSet<NotaFiscalItem> NotaFiscalItem { get; set; }

        public DbSet<NotaFiscalModelo> NotaFiscalModelo { get; set; }

        public DbSet<Operacao> Operacao { get; set; }

        //public DbSet<Ocorrencia> Ocorrencia { get; set; }

        public DbSet<OrdemCarga> OrdemCarga { get; set; }

        public DbSet<OrdemCargaAndamento> OrdemCargaAndamento { get; set; }

        public DbSet<OrdemCargaNotaFiscal> OrdemCargaNotaFiscal { get; set; }

        public DbSet<OrdemCargaStatus> OrdemCargaStatus { get; set; }

        //public DbSet<Pagamento> Pagamento { get; set; }

        //public DbSet<PagamentoDetalhe> PagamentoDetalhe { get; set; }

        public DbSet<PedidoVenda> PedidoVenda { get; set; }

        public DbSet<PedidoVendaAndamento> PedidoVendaAndamento { get; set; }

        public DbSet<PedidoVendaAndamentoWorkflow> PedidoVendaAndamentoWorkflow { get; set; }

        public DbSet<PedidoVendaItem> PedidoVendaItem { get; set; }

        public DbSet<PedidoVendaItemConferenciaItem> PedidoVendaItemConferenciaItem { get; set; }

        public DbSet<PedidoVendaItemNotaFiscalItem> PedidoVendaItemNotaFiscalItem { get; set; }

        public DbSet<PedidoVendaPagamento> PedidoVendaPagamento { get; set; }

        public DbSet<PedidoVendaStatus> PedidoVendaStatus { get; set; }

        public DbSet<Pessoa> Pessoa { get; set; }

        public DbSet<PessoaArquivo> PessoaArquivo { get; set; }

        public DbSet<PessoaContato> PessoaContato { get; set; }

        public DbSet<PessoaEndereco> PessoaEndereco { get; set; }

        public DbSet<PessoaPerfil> PessoaPerfil { get; set; }

        public DbSet<PessoaSegmento> PessoaSegmento { get; set; }

        public DbSet<PessoaVendedor> PessoaVendedor { get; set; }

        public DbSet<PlanoConta> PlanoConta { get; set; }

        public DbSet<PlanoContaTipo> PlanoContaTipo { get; set; }

        public DbSet<Preco> Preco { get; set; }

        public DbSet<Produto> Produto { get; set; }

        public DbSet<ProdutoAtributo> ProdutoAtributo { get; set; }

        public DbSet<ProdutoCombinacao> ProdutoCombinacao { get; set; }

        public DbSet<ProdutoCombinacaoAtributo> ProdutoCombinacaoAtributo { get; set; }

        public DbSet<ProdutoCSOSN> ProdutoCSOSN { get; set; }

        public DbSet<ProdutoCST> ProdutoCST { get; set; }

        public DbSet<ProdutoFornecedor> ProdutoFornecedor { get; set; }

        public DbSet<ProdutoPreco> ProdutoPreco { get; set; }

        public DbSet<ProdutoTributacao> ProdutoTributacao { get; set; }

        public DbSet<Requisicao> Requisicao { get; set; }

        public DbSet<RequisicaoItem> RequisicaoItem { get; set; }

        public DbSet<Titulo> Titulo { get; set; }

        public DbSet<TituloDetalhe> TituloDetalhe { get; set; }

        public DbSet<Tributacao> Tributacao { get; set; }

        public DbSet<UnidadeMedida> UnidadeMedida { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<UsuarioEmail> UsuarioEmail { get; set; }

        public DbSet<Variacao> Variacao { get; set; }

        //public DbSet<Veiculo> Veiculo { get; set; }

        //public DbSet<Venda> Venda { get; set; }

        //public DbSet<VendaItem> VendaItem { get; set; }

        //public DbSet<VendaItemEstoqueSaida> VendaItemEstoqueSaida { get; set; }

        //public DbSet<VendaStatus> VendaStatus { get; set; }

        //public DbSet<VendaStatusWorkflow> VendaStatusWorkflow { get; set; }

        //public DbSet<Viagem> Viagem { get; set; }

        //public DbSet<ViagemGrupo> ViagemGrupo { get; set; }

        //public DbSet<ViagemGrupoCTe> ViagemGrupoCTe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<UsuarioEmail>()
                .HasOne(x => x.Usuario)
                .WithMany(e => e.UsuarioEmail)
                .HasForeignKey(x => x.UsuarioID);



            modelBuilder.Entity<PessoaVendedor>()
                .HasOne(x => x.Pessoa)
                .WithMany(e => e.Vendedores)
                .HasForeignKey(x => x.PessoaID);

            modelBuilder.Entity<PessoaVendedor>()
                .HasOne(x => x.Vendedor)
                .WithMany(e => e.Clientes)
                .HasForeignKey(x => x.VendedorID);



            modelBuilder.Entity<PedidoVendaAndamentoWorkflow>()
                .HasOne(x => x.PedidoVendaStatus)
                .WithMany(e => e.PedidoVendaAndamentoWorkflow)
                .HasForeignKey(x => x.PedidoVendaStatusID);

            modelBuilder.Entity<PedidoVendaAndamentoWorkflow>()
                .HasOne(x => x.PedidoVendaStatusPara)
                .WithMany(e => e.PedidoVendaAndamentoWorkflowPara)
                .HasForeignKey(x => x.PedidoVendaStatusParaID);



            modelBuilder.Entity<TituloDetalhe>().Property(c => c.vTotal).HasPrecision(18, 3);

            modelBuilder.Entity<TituloDetalhe>().Property(c => c.pDesconto).HasPrecision(18, 3);
            modelBuilder.Entity<TituloDetalhe>().Property(c => c.vDesconto).HasPrecision(18, 3);

            modelBuilder.Entity<TituloDetalhe>().Property(c => c.pJuros).HasPrecision(18, 3);
            modelBuilder.Entity<TituloDetalhe>().Property(c => c.vJuros).HasPrecision(18, 3);

            modelBuilder.Entity<TituloDetalhe>().Property(c => c.pMulta).HasPrecision(18, 3);
            modelBuilder.Entity<TituloDetalhe>().Property(c => c.vMulta).HasPrecision(18, 3);

            modelBuilder.Entity<TituloDetalhe>().Property(c => c.vLiquido).HasPrecision(18, 3);



            modelBuilder.Entity<ContaBancariaFormaPagamento>().Property(c => c.Juros).HasPrecision(18, 3);
            modelBuilder.Entity<ContaBancariaFormaPagamento>().Property(c => c.Multa).HasPrecision(18, 3);


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false).UseSqlServer(@"Server=localhost\AtlantaSistemas;Database=Dev;User Id=sa;Password=@Rped94ft");
        }
    }

}