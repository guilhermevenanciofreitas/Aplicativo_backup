using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Model
{

    [Serializable()]
    [Table("Estado")]
    public partial class Estado
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