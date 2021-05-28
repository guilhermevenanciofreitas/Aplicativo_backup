using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("CentroCusto")]
    public partial class CentroCusto : _Extends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? CentroCustoID { get; set; }

        [StringLength(140)]
        public string Descricao { get; set; }

        public bool Ativo { get; set; } = true;

    }
}