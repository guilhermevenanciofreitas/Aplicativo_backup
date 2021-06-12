using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PedidoVenda")]
    public partial class PedidoVenda : _Extends
    {

        public PedidoVenda()
        {
            PedidoVendaItem = new HashSet<PedidoVendaItem>();
            PedidoVendaPagamento = new HashSet<PedidoVendaPagamento>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PedidoVendaID { get; set; }

        //[ForeignKey(" PedidoVendaStatus")]
        //public int? PedidoVendaStatusID { get; set; }


        [ForeignKey("Cliente")]
        public int? ClienteID { get; set; }

        [ForeignKey("Vendedor")]
        public int? VendedorID { get; set; }

        public DateTime? Data { get; set; }

        public DateTime? Finalizado { get; set; }

        public DateTime? Faturamento { get; set; }

        public DateTime? Conferencia { get; set; }

        public DateTime? Expedicao { get; set; }

        public DateTime? Entregue { get; set; }

        [ForeignKey("Transportadora")]
        public int? TransportadoraID { get; set; }

        [StringLength(500)]
        public string Observacao { get; set; }

        public virtual Pessoa Vendedor { get; set; }

        public virtual Pessoa Cliente { get; set; }

        public virtual Pessoa Transportadora { get; set; }

        public ICollection<PedidoVendaItem> PedidoVendaItem { get; set; }

        public ICollection<PedidoVendaPagamento> PedidoVendaPagamento { get; set; }

    }
}