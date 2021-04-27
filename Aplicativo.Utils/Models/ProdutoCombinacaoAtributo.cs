using Aplicativo.Utils.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("ProdutoCombinacaoAtributo")]
    public partial class ProdutoCombinacaoAtributo : _Extends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoCombinacaoAtributoID { get; set; }

        [ForeignKey("ProdutoCombinacao")]
        public int? ProdutoCombinacaoID { get; set; }

        [ForeignKey("Atributo")]
        public int? AtributoID { get; set; }

        [ForeignKey("Variacao")]
        public int? VariacaoID { get; set; }

        //Extenção para DropDownList
        [NotMapped]
        public int? OptVariacaoID { get; set; }

        public virtual ProdutoCombinacao ProdutoCombinacao { get; set; }

        public virtual Atributo Atributo { get; set; }

        public virtual Variacao Variacao { get; set; }

    }
}