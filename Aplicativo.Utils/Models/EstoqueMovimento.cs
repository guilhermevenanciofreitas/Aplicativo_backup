using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    public enum EstoqueMovimentoOrigem
    {
        Ajuste = 1,
        EntradaMercadoria = 2,
        Transferencia = 3,
    }

    public enum EstoqueMovimentoTipo
    {
        Entrada = 1,
        Saida = 2,
    }

    [Serializable()]
    [Table("EstoqueMovimento")]
    public partial class EstoqueMovimento : _Extends
    {

        public EstoqueMovimento()
        {
            EstoqueMovimentoItem = new HashSet<EstoqueMovimentoItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EstoqueMovimentoID { get; set; }

        [ForeignKey("Estoque")]
        public int? EstoqueID { get; set; }

        public EstoqueMovimentoOrigem? EstoqueOrigemID { get; set; }

        public EstoqueMovimentoTipo? EstoqueMovimentoTipoID { get; set; }

        public DateTime? Data { get; set; }

        [ForeignKey("Funcionario")]
        public int? FuncionarioID { get; set; }

        [StringLength(300)]
        public string Observacao { get; set; }

        public virtual Estoque Estoque { get; set; }

        public virtual Pessoa Funcionario { get; set; }

        public virtual ICollection<EstoqueMovimentoItem> EstoqueMovimentoItem { get; set; }

    }
}