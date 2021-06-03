using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("FormaPagamento")]
    public partial class FormaPagamento : _Extends
    {

        public FormaPagamento()
        {
            ContaBancariaFormaPagamento = new HashSet<ContaBancariaFormaPagamento>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? FormaPagamentoID { get; set; }

        [StringLength(200)]
        public string Descricao { get; set; }

        public bool? Ativo { get; set; } = true;

        public virtual ICollection<ContaBancariaFormaPagamento> ContaBancariaFormaPagamento { get; set; }

    }
}