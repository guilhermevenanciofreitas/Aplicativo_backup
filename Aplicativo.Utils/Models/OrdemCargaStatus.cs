using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("OrdemCargaStatus")]
    public partial class OrdemCargaStatus
    {
 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrdemCargaStatusID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        public bool Ativo { get; set; } = true;

    }
}