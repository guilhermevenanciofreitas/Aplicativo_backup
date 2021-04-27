using Aplicativo.Utils.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("ProdutoCSOSN")]
    public partial class ProdutoCSOSN : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoCSOSNID { get; set; }

        [StringLength(3)]
        public string Codigo { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

    }
}