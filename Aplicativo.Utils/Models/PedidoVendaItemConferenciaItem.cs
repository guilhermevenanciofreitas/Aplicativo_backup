using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PedidoVendaItemConferenciaItem")]
    public partial class PedidoVendaItemConferenciaItem : _Extends
    {

        public PedidoVendaItemConferenciaItem()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PedidoVendaItemConferenciaItemID { get; set; }

        [ForeignKey("PedidoVendaItem")]
        public int? PedidoVendaItemID { get; set; }

        [ForeignKey("ConferenciaItem")]
        public int? ConferenciaItemID { get; set; }

        public virtual PedidoVendaItem PedidoVendaItem { get; set; }

        public virtual ConferenciaItem ConferenciaItem { get; set; }

    }
}