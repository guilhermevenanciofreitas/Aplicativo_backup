using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PedidoVendaPagamento")]
    public partial class PedidoVendaPagamento : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PedidoVendaPagamentoID { get; set; }

        [ForeignKey("PedidoVenda")]
        public int? PedidoVendaID { get; set; }

        [ForeignKey("Titulo")]
        public int? TituloID { get; set; }

        public virtual PedidoVenda PedidoVenda { get; set; }

        public virtual Titulo Titulo { get; set; }

    }
}