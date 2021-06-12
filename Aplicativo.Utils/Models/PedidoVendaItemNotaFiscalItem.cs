using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("PedidoVendaItemNotaFiscalItem")]
    public partial class PedidoVendaItemNotaFiscalItem
    {
 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PedidoVendaItemNotaFiscalItemID { get; set; }

        [ForeignKey("PedidoVendaItem")]
        public int? PedidoVendaItemID { get; set; }

        [ForeignKey("NotaFiscalItem")]
        public int? NotaFiscalItemID { get; set; }

        public virtual PedidoVendaItem PedidoVendaItem { get; set; }

        public virtual NotaFiscalItem NotaFiscalItem { get; set; }

    }
}