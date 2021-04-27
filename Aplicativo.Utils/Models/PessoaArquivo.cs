using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("PessoaArquivo")]
    public partial class PessoaArquivo : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PessoaArquivoID { get; set; }

        [ForeignKey("Pessoa")]
        public int? PessoaID { get; set; }

        [ForeignKey("Arquivo")]
        public int? ArquivoID { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public virtual Arquivo Arquivo { get; set; }

    }
}