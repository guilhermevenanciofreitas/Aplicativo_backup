using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PedidoVendaAndamento")]
    public partial class PedidoVendaAndamento : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PedidoVendaAndamentoID { get; set; }
        
        [ForeignKey("PedidoVenda")]
        public int? PedidoVendaID { get; set; }

        [ForeignKey("Usuario")]
        public int? UsuarioID { get; set; }

        public DateTime? Data { get; set; }

        [ForeignKey("PedidoVendaStatus")]
        public int? PedidoVendaStatusID { get; set; }

        [StringLength(200)]
        public string Observacao { get; set; }



        public virtual PedidoVenda PedidoVenda { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual PedidoVendaStatus PedidoVendaStatus { get; set; }

    }
}