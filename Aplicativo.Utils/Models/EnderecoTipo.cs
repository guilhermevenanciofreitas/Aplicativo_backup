using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("EnderecoTipo")]
    public partial class EnderecoTipo : _Extends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EnderecoTipoID { get; set; }

        [StringLength(140)]
        public string Descricao { get; set; }

    }
}