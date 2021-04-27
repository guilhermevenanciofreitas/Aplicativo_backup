using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("UnidadeMedida")]
    public partial class UnidadeMedida : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UnidadeMedidaID { get; set; }

        [StringLength(20)]
        public string Unidade { get; set; }

        [StringLength(200)]
        public string Descricao { get; set; }

        public bool? Ativo { get; set; }

    }
}