using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("EstoqueMovimentoItemSaida")]
    public partial class EstoqueMovimentoItemSaida : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EstoqueMovimentoItemSaidaID { get; set; }

        public int? EstoqueMovimentoItemID { get; set; }

    }
}