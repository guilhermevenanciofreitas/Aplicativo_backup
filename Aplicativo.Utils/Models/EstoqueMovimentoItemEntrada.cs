using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("EstoqueMovimentoItemEntrada")]
    public partial class EstoqueMovimentoItemEntrada : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EstoqueMovimentoItemEntradaID { get; set; }

        public int? CodigoBarra { get; set; }

        public decimal? Saldo { get; set; }

    }
}