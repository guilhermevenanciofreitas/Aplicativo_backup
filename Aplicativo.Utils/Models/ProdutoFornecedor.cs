using Aplicativo.Utils.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("ProdutoFornecedor")]
    public partial class ProdutoFornecedor : _Extends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoFornecedorID { get; set; }

        [ForeignKey("Produto")]
        public int? ProdutoID { get; set; }

        [ForeignKey("Fornecedor")]
        public int? FornecedorID { get; set; }

        [StringLength(30)]
        public string CodigoFornecedor { get; set; }

        [ForeignKey("UnidadeMedida")]
        public int? UnidadeMedidaID { get; set; }

        public decimal? Contem { get; set; }

        public decimal? Preco { get; set; }

        [NotMapped]
        public decimal? Total => (Contem ?? 0) * (Preco ?? 0);

        public virtual Produto Produto { get; set; }

        public virtual Pessoa Fornecedor { get; set; }

        public virtual UnidadeMedida UnidadeMedida { get; set; }

    }
}