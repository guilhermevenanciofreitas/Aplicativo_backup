using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Serializable()]
    [Table("Pagamento")]
    public partial class Pagamento
    {
        public Pagamento()
        {
            PagamentoDetalhe = new HashSet<PagamentoDetalhe>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PagamentoID { get; set; }

        public DateTime? DataLancamento { get; set; } = DateTime.Now;

        public DateTime? DataPagamento { get; set; } = DateTime.Now;

        public virtual ICollection<PagamentoDetalhe> PagamentoDetalhe { get; set; }

    }
}