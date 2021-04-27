using Aplicativo.Utils.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("ProdutoAtributo")]
    public partial class ProdutoAtributo : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoAtributoID { get; set; }

        [ForeignKey("Produto")]
        public int? ProdutoID { get; set; }

        [ForeignKey("Atributo")]
        public int? AtributoID { get; set; }

        public virtual Produto Produto { get; set; }

        public virtual Atributo Atributo { get; set; }

    }
}