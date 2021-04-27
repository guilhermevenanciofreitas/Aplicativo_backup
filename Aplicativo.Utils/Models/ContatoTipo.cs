using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("ContatoTipo")]
    public partial class ContatoTipo : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ContatoTipoID { get; set; }

        [StringLength(140)]
        public string Descricao { get; set; }

    }
}