using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Serializable()]
    [Table("Titulo")]
    public partial class Titulo : _Extends
    {
        public Titulo()
        {
            TituloDetalhe = new HashSet<TituloDetalhe>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TituloID { get; set; }

        public DateTime? DataLancamento { get; set; } = DateTime.Now;

        public bool? Ativo { get; set; } = true;

        public virtual ICollection<TituloDetalhe> TituloDetalhe { get; set; }

    }
}