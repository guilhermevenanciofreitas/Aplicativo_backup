using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("FormaPagamento")]
    public partial class FormaPagamento : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? FormaPagamentoID { get; set; }

        [StringLength(200)]
        public string Descricao { get; set; }

        public bool? Ativo { get; set; } = true;

    }
}