using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Certificado")]
    public partial class Certificado : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? CertificadoID { get; set; }

        [StringLength(600)]
        public string Nome { get; set; }

        [StringLength(100)]
        public string Serial { get; set; }

        public DateTime? Expira { get; set; }

        [StringLength(50)]
        public string Senha { get; set; }

        [ForeignKey("Arquivo")]
        public int? ArquivoID { get; set; }

        public virtual Arquivo Arquivo { get; set; }


    }
}