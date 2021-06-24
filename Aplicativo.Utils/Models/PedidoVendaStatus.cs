using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PedidoVendaStatus")]
    public partial class PedidoVendaStatus : _Extends
    {

        public PedidoVendaStatus()
        {
            PedidoVendaAndamentoWorkflow = new HashSet<PedidoVendaAndamentoWorkflow>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PedidoVendaStatusID { get; set; }
        
        [StringLength(100)]
        public string Descricao { get; set; }

        public bool? IsFinalizado { get; set; }

        public bool? IsSeparado { get; set; }

        public bool? IsConferido { get; set; }

        public bool? IsFaturado { get; set; }

        public bool? IsExpedicao { get; set; }

        public bool? IsEntregue { get; set; }

        public virtual ICollection<PedidoVendaAndamentoWorkflow> PedidoVendaAndamentoWorkflow { get; set; }

        public virtual ICollection<PedidoVendaAndamentoWorkflow> PedidoVendaAndamentoWorkflowPara { get; set; }

    }
}