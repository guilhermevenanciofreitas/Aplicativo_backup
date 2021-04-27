using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("ContaBancariaFormaPagamento")]
    public partial class ContaBancariaFormaPagamento : _Extends
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ContaBancariaFormaPagamentoID { get; set; }

        [ForeignKey("ContaBancaria")]
        public int? ContaBancariaID { get; set; }

        [ForeignKey("FormaPagamento")]
        public int? FormaPagamentoID { get; set; }

        public decimal? pJuros { get; set; }

        public decimal? pMulta { get; set; }


        public virtual ContaBancaria ContaBancaria { get; set; }

        public virtual FormaPagamento FormaPagamento { get; set; }

    }
}