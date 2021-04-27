using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Atributo")]
    public partial class Atributo : _Extends
    {
        public Atributo()
        {
            Variacao = new HashSet<Variacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? AtributoID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        public virtual ICollection<Variacao> Variacao { get; set; }

    }

}