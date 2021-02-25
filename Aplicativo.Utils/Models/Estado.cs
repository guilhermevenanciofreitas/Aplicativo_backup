using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Estado")]
    public partial class Estado : _Extends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EstadoID { get; set; }

        [StringLength(120)]
        public string Nome { get; set; }

        [StringLength(2)]
        public string UF { get; set; }
    }

}