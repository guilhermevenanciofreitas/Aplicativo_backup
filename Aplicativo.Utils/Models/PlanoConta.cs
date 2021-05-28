using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Serializable()]
    [Table("PlanoConta")]
    public partial class PlanoConta : _Extends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PlanoContaID { get; set; }

        [StringLength(140)]
        public string Descricao { get; set; }

        [ForeignKey("PlanoContaTipo")]
        public int? PlanoContaTipoID { get; set; } //1 - Entrada | 2 - Saída

        public int? PlanoContaGrupoID { get; set; }

        public bool Ativo { get; set; } = true;

        public virtual PlanoContaTipo PlanoContaTipo { get; set; }

    }
}