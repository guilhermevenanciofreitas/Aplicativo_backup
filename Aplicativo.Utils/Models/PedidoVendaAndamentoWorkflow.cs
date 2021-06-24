using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PedidoVendaAndamentoWorkflow")]
    public partial class PedidoVendaAndamentoWorkflow : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PedidoVendaAndamentoWorkflowID { get; set; }
        
        [ForeignKey("PedidoVendaStatus")]
        public int? PedidoVendaStatusID { get; set; }

        [ForeignKey("PedidoVendaStatusPara")]
        public int? PedidoVendaStatusParaID { get; set; }


        public virtual PedidoVendaStatus PedidoVendaStatus { get; set; }

        public virtual PedidoVendaStatus PedidoVendaStatusPara { get; set; }

    }
}