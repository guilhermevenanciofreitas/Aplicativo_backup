using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("PessoaSegmento")]
    public partial class PessoaSegmento : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PessoaSegmentoID { get; set; }

        [StringLength(120)]
        public string Descricao { get; set; }

    }
}