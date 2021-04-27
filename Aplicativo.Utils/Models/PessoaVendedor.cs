using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("PessoaVendedor")]
    public partial class PessoaVendedor : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PessoaVendedorID { get; set; }

        [ForeignKey("Pessoa")]
        public int? PessoaID { get; set; }

        [ForeignKey("Vendedor")]
        public int? VendedorID { get; set; }


        public virtual Pessoa Pessoa { get; set; }

        public virtual Pessoa Vendedor { get; set; }

    }
}