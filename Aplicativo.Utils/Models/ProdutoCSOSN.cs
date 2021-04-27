using Aplicativo.Utils.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("ProdutoCST")]
    public partial class ProdutoCST : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoCSTID { get; set; }

        [StringLength(2)]
        public string Codigo { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

    }
}