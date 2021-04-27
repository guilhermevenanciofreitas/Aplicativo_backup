using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("PessoaContato")]
    public partial class PessoaContato : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PessoaContatoID { get; set; }

        [ForeignKey("Pessoa")]
        public int? PessoaID { get; set; }

        [ForeignKey("Contato")]
        public int? ContatoID { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public virtual Contato Contato { get; set; }

    }
}