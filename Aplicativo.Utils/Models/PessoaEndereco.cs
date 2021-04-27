using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("PessoaEndereco")]
    public partial class PessoaEndereco : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PessoaEnderecoID { get; set; }

        [ForeignKey("Pessoa")]
        public int? PessoaID { get; set; }

        [ForeignKey("Endereco")]
        public int? EnderecoID { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public virtual Endereco Endereco { get; set; }

    }
}