using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PagamentoDetalhe")]
    public partial class PagamentoDetalhe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PagamentoDetalheID { get; set; }

        [ForeignKey("Pagamento")]
        public int? PagamentoID { get; set; }

        [ForeignKey("ContaBancariaHistorico")]
        public int? ContaBancariaHistoricoID { get; set; }

        [ForeignKey("Pessoa")]
        public int? PessoaID { get; set; }

        [ForeignKey("FormaPagamento")]
        public int? FormaPagamentoID { get; set; }

        [ForeignKey("PlanoConta")]
        public int? PlanoContaID { get; set; }

        [ForeignKey("CentroCusto")]
        public int? CentroCustoID { get; set; }

        public decimal? vPago { get; set; } = 0;


        public virtual Pagamento Pagamento { get; set; }

        public virtual ContaBancariaFechamento ContaBancariaHistorico { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        //public virtual FormaPagamento FormaPagamento { get; set; }

        //public virtual PlanoConta PlanoConta { get; set; }

        //public virtual CentroCusto CentroCusto { get; set; }

    }
}