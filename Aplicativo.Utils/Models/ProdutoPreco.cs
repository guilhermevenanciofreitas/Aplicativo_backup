using Aplicativo.Utils.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("ProdutoPreco")]
    public partial class ProdutoPreco : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoPrecoID { get; set; }

        [ForeignKey("Produto")]
        public int? ProdutoID { get; set; }

        [ForeignKey("Preco")]
        public int? PrecoID { get; set; }

        public decimal? Valor { get; set; }

        public virtual Produto Produto { get; set; }

        public virtual Preco Preco { get; set; }

    }
}