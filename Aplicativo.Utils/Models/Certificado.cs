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

        [StringLength(800)]
        public string Descricao { get; set; }

        [ForeignKey("Arquivo")]
        public int? ArquivoID { get; set; }

        public virtual Arquivo Arquivo { get; set; }


    }
}