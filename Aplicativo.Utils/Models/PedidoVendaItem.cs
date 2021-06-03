using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PedidoVendaItem")]
    public partial class PedidoVendaItem : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PedidoVendaItemID { get; set; }

        [ForeignKey("PedidoVenda")]
        public int? PedidoVendaID { get; set; }

        [ForeignKey("Produto")]
        public int? ProdutoID { get; set; }

        public decimal? Quantidade { get; set; }

        public decimal? vPreco { get; set; }

        public decimal? vDesconto { get; set; }

        public decimal? pDesconto { get; set; }

        public decimal? DescontoTotal { get; set; }

        public decimal? vTotal { get; set; }

        public virtual PedidoVenda PedidoVenda { get; set; }

        public virtual Produto Produto { get; set; }

    }
}