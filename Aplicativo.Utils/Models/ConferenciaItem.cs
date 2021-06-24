using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("ConferenciaItem")]
    public partial class ConferenciaItem : _Extends
    {

        public ConferenciaItem()
        {
            PedidoVendaItemConferenciaItem = new HashSet<PedidoVendaItemConferenciaItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ConferenciaItemID { get; set; }

        [ForeignKey("Conferencia")]
        public int? ConferenciaID { get; set; }

        [ForeignKey("Produto")]
        public int? ProdutoID { get; set; }

        [ForeignKey("EstoqueMovimentoItem")]
        public int? EstoqueMovimentoItemID { get; set; }

        public decimal? Quantidade { get; set; }


        public virtual Conferencia Conferencia { get; set; }

        public virtual Produto Produto { get; set; }

        public virtual EstoqueMovimentoItem EstoqueMovimentoItem { get; set; }

        public virtual ICollection<PedidoVendaItemConferenciaItem> PedidoVendaItemConferenciaItem { get; set; }

    }
}