using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("EstoqueMovimentoItemSaida")]
    public partial class EstoqueMovimentoItemSaida : _Extends
    {

        public EstoqueMovimentoItemSaida()
        {
            EstoqueMovimentoItem = new HashSet<EstoqueMovimentoItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EstoqueMovimentoItemSaidaID { get; set; }

        [ForeignKey("EstoqueMovimentoItemEntrada")]
        public int? EstoqueMovimentoItemEntradaID { get; set; }

        public virtual EstoqueMovimentoItemEntrada EstoqueMovimentoItemEntrada { get; set; }

        public virtual ICollection<EstoqueMovimentoItem> EstoqueMovimentoItem { get; set; }

    }
}