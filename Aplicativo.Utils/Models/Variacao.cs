using Aplicativo.Utils.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("Variacao")]
    public partial class Variacao : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? VariacaoID { get; set; }

        [ForeignKey("Atributo")]
        public int? AtributoID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        public virtual Atributo Atributo { get; set; }

    }
}