using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("RequisicaoItem")]
    public partial class RequisicaoItem : _Extends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RequisicaoItemID { get; set; }
        
        [ForeignKey("Requisicao")]
        public int? RequisicaoID { get; set; }

        [ForeignKey("Produto")]
        public int? ProdutoID { get; set; }

        public decimal? Quantidade { get; set; }

        [ForeignKey("EstoqueMovimentoItemEntrada")]
        public int? EstoqueMovimentoItemEntradaID { get; set; }

        public DateTime? DataSaida { get; set; }

        public DateTime? DataEntrada { get; set; }

        public virtual Requisicao Requisicao { get; set; }

        public virtual Produto Produto { get; set; }

        public virtual EstoqueMovimentoItemEntrada EstoqueMovimentoItemEntrada { get; set; }

    }
}