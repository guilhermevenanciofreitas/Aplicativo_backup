using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Tributacao")]
    public partial class Tributacao : _Extends
    {
        public Tributacao()
        {
            TributacaoOperacao = new HashSet<TributacaoOperacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TributacaoID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        [StringLength(2)]
        public string ICMS_CST { get; set; }

        [StringLength(3)]
        public string ICMS_CSOSN { get; set; }

        public decimal? ICMS_Aliq { get; set; }


        [StringLength(2)]
        public string IPI_CST { get; set; }

        public decimal? IPI_Aliq { get; set; }

        [StringLength(2)]
        public string PIS_CST { get; set; }

        public decimal? PIS_Aliq { get; set; }


        [StringLength(2)]
        public string COFINS_CST { get; set; }

        public decimal? COFINS_Aliq { get; set; }

        public virtual ICollection<TributacaoOperacao> TributacaoOperacao { get; set; }

    }
}