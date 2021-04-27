using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Serializable()]
    [Table("TituloDetalhe")]
    public partial class TituloDetalhe : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TituloDetalheID { get; set; }

        [ForeignKey("Titulo")]
        public int? TituloID { get; set; }

        [StringLength(60)]
        public string nDocumento { get; set; }

        [NotMapped]
        public string nDocumentoParcela => string.IsNullOrEmpty(nDocumento) ? null : nDocumento + "-" + nParcela;

        public DateTime? DataEmissao { get; set; } = DateTime.Now;

        public DateTime? DataVencimento { get; set; }

        public DateTime? DataCancelamento { get; set; }

        public int? nParcela { get; set; } = 1;

        [NotMapped]
        public int? nTotalParcela { get; set; } = 1;

        [ForeignKey("Pessoa")]
        public int? PessoaID { get; set; }

        [ForeignKey("ContaBancaria")]
        public int? ContaBancariaID { get; set; }

        [ForeignKey("FormaPagamento")]
        public int? FormaPagamentoID { get; set; }

        [ForeignKey("PlanoContaID")]
        public int? PlanoContaID { get; set; }

        [ForeignKey("CentroCusto")]
        public int? CentroCustoID { get; set; }

        public decimal? vTotal { get; set; } = 0;

        public decimal? pDesconto { get; set; } = 0;

        public decimal? vDesconto { get; set; } = 0;

        public decimal? pJuros { get; set; } = 0;

        public decimal? vJuros { get; set; } = 0;

        public int? DiasAtrasados { get; set; } = 0;

        public decimal? pMulta { get; set; } = 0;

        public decimal? vMulta { get; set; } = 0;

        public decimal? vLiquido { get; set; } = 0;

        [ForeignKey("Pagamento")]
        public int? PagamentoID { get; set; }

        [StringLength(200)]
        public string Observacao { get; set; }
        

        public virtual Titulo Titulo { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public virtual ContaBancaria ContaBancaria { get; set; }

        public virtual FormaPagamento FormaPagamento { get; set; }

        //public virtual PlanoConta PlanoConta { get; set; }

        //public virtual CentroCusto CentroCusto { get; set; }

        //public virtual Pagamento Pagamento { get; set; }


    }
}