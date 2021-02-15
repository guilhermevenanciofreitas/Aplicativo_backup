using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Model
{

    [Serializable()]
    [Table("EnderecoTipo")]
    public partial class EnderecoTipo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EnderecoTipoID { get; set; }

        [StringLength(140)]
        public string Descricao { get; set; }

    }
}