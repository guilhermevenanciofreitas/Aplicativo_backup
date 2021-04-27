using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Estoque")]
    public partial class Estoque : _Extends
    {
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EstoqueID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

    }
}