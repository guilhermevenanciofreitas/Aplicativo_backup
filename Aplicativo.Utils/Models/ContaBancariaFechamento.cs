using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("ContaBancariaFechamento")]
    public partial class ContaBancariaFechamento
    {

        public ContaBancariaFechamento()
        {
            PagamentoDetalhe = new HashSet<PagamentoDetalhe>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ContaBancariaHistoricoID { get; set; }

        [ForeignKey("ContaBancaria")]
        public int? ContaBancariaID { get; set; }

        public int? OperadorID { get; set; }

        public DateTime? DataAbertura { get; set; }

        public DateTime? DataFechamento { get; set; }


        public virtual ContaBancaria ContaBancaria { get; set; }

        public virtual Usuario Operador { get; set; }

        public virtual ICollection<PagamentoDetalhe> PagamentoDetalhe { get; set; }

    }
}