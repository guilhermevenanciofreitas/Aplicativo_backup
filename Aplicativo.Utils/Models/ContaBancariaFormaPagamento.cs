using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Aplicativo.Utils.Models
{

    public enum TipoCobranca
    {
        Percentual = 1,
        ValorFixo = 2,
    }

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


        public TipoCobranca? TipoTaxa { get; set; }

        public decimal? Taxa { get; set; }

        public TipoCobranca? TipoJuros { get; set; }

        public decimal? Juros { get; set; }

        public TipoCobranca? TipoMulta { get; set; }

        public decimal? Multa { get; set; }


        public virtual ContaBancaria ContaBancaria { get; set; }

        public virtual FormaPagamento FormaPagamento { get; set; }

    }
}