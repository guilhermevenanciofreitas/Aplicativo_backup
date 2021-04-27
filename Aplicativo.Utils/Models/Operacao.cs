using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    public enum OperacaoTipo
    {
        Entrada = 1,
        Saida = 2,
    }

    [Serializable()]
    [Table("Operacao")]
    public partial class Operacao : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OperacaoID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        public OperacaoTipo? OperacaoTipoID { get; set; }

    }
}