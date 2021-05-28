using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("PlanoContaTipo")]
    public partial class PlanoContaTipo : _Extends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PlanoContaTipoID { get; set; }

        [StringLength(140)]
        public string Descricao { get; set; }

    }
}