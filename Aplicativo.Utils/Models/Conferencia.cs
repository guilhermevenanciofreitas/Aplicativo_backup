using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("Conferencia")]
    public partial class Conferencia : _Extends
    {

        public Conferencia()
        {
            ConferenciaItem = new HashSet<ConferenciaItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ConferenciaID { get; set; }

        [ForeignKey("Funcionario")]
        public int? FuncionarioID { get; set; }

        public DateTime? Data { get; set; }

        public virtual Pessoa Funcionario { get; set; }

        public ICollection<ConferenciaItem> ConferenciaItem { get; set; }

    }
}