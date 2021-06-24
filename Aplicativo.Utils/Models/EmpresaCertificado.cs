using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("EmpresaCertificado")]
    public partial class EmpresaCertificado : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EmpresaCertificadoID { get; set; }

        [ForeignKey("Empresa")]
        public int? EmpresaID { get; set; }

        [ForeignKey("Certificado")]
        public int? CertificadoID { get; set; }


        public virtual Empresa Empresa { get; set; }

        public virtual Certificado Certificado { get; set; }

    }
}