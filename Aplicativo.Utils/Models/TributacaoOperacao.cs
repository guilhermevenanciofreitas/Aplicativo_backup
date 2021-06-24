using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("TributacaoOperacao")]
    public partial class TributacaoOperacao : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TributacaoOperacaoID { get; set; }

        [ForeignKey("Tributacao")]
        public int? TributacaoID { get; set; }

        [ForeignKey("Operacao")]
        public int? OperacaoID { get; set; }

        [ForeignKey("CFOP_Estadual")]
        [StringLength(5)]
        public string Codigo_CFOP_Estadual { get; set; }

        [ForeignKey("CFOP_Interestadual")]
        [StringLength(5)]
        public string Codigo_CFOP_Interestadual { get; set; }

        [ForeignKey("CFOP_Exterior")]
        [StringLength(5)]
        public string Codigo_CFOP_Exterior { get; set; }


        public virtual Tributacao Tributacao { get; set; }

        public virtual Operacao Operacao { get; set; }

        public virtual CFOP CFOP_Estadual { get; set; }

        public virtual CFOP CFOP_Interestadual { get; set; }

        public virtual CFOP CFOP_Exterior { get; set; }

    }
}