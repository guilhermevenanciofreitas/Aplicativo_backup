using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("EstoqueMovimentoItemEntrada")]
    public partial class EstoqueMovimentoItemEntrada : _Extends
    {

        public EstoqueMovimentoItemEntrada()
        {
            EstoqueMovimentoItem = new HashSet<EstoqueMovimentoItem>();
            EstoqueMovimentoItemSaida = new HashSet<EstoqueMovimentoItemSaida>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EstoqueMovimentoItemEntradaID { get; set; }

        public long? CodigoBarra { get; set; }

        public decimal? Saldo { get; set; }

        public virtual ICollection<EstoqueMovimentoItem> EstoqueMovimentoItem { get; set; }

        public virtual ICollection<EstoqueMovimentoItemSaida> EstoqueMovimentoItemSaida { get; set; }

    }
}