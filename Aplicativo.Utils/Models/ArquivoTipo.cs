using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("ArquivoTipo")]
    public partial class ArquivoTipo : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ArquivoTipoID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

    }
}