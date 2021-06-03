using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Aplicativo.Utils.Models
{

    public enum TipoID
    {
        Caixa,
        Banco,
    }

    [Serializable()]
    [Table("ContaBancaria")]
    public partial class ContaBancaria : _Extends
    {
        public ContaBancaria()
        {
            ContaBancariaFechamento = new HashSet<ContaBancariaFechamento>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ContaBancariaID { get; set; }

        [StringLength(120)]
        public string Descricao { get; set; }

        public decimal? Saldo { get; set; } = 0;

        public TipoID? TipoID { get; set; }

        public bool? Ativo { get; set; } = true;

        [NotMapped]
        public string Situacao
        {
            get
            {
                if (ContaBancariaFechamento == null)
                    return "";

                if (ContaBancariaFechamento.Where(c => c.DataFechamento == null).Count() == 0)
                    return "Fechado";
                else
                    return "Aberto";
            }
        }

        public virtual ICollection<ContaBancariaFechamento> ContaBancariaFechamento { get; set; }

        public virtual ICollection<ContaBancariaFormaPagamento> ContaBancariaFormaPagamento { get; set; }

    }
}