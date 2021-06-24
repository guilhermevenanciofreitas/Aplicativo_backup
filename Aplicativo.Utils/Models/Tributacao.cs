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

        [ForeignKey("CST_ICMS")]
        [StringLength(2)]
        public string Codigo_CST { get; set; }

        [ForeignKey("CSOSN_ICMS")]
        [StringLength(3)]
        public string Codigo_CSOSN { get; set; }

        public decimal? Aliq_ICMS { get; set; }


        [ForeignKey("CST_IPI")]
        [StringLength(2)]
        public string Codigo_IPI { get; set; }

        public decimal? Aliq_IPI { get; set; }

        [ForeignKey("CST_PIS")]
        [StringLength(2)]
        public string Codigo_PIS { get; set; }

        public decimal? Aliq_PIS { get; set; }


        [ForeignKey("CST_COFINS")]
        [StringLength(2)]
        public string Codigo_COFINS { get; set; }

        public decimal? Aliq_COFINS { get; set; }

        public bool? Ativo { get; set; } = true;



        [NotMapped]
        public string ICMS => "(" + Math.Round(Aliq_ICMS ?? 0, 2) + "%) " + Codigo_CST + " - " + CST_ICMS?.Descricao;

        [NotMapped]
        public string CSOSN =>  "(" + Math.Round(Aliq_ICMS ?? 0, 2) + "%) " + Codigo_CSOSN + " - " + CSOSN_ICMS?.Descricao;

        [NotMapped]
        public string IPI => "(" + Math.Round(Aliq_IPI ?? 0, 2) + "%) " + Codigo_IPI + " - " + CST_IPI?.Descricao;

        [NotMapped]
        public string PIS => "(" + Math.Round(Aliq_PIS ?? 0, 2) + "%) " + Codigo_PIS + " - " + CST_PIS?.Descricao;

        [NotMapped]
        public string COFINS => "(" + Math.Round(Aliq_COFINS ?? 0, 2) + "%) " + Codigo_COFINS + " - " + CST_COFINS?.Descricao;


        public virtual CST_ICMS CST_ICMS { get; set; }

        public virtual CSOSN_ICMS CSOSN_ICMS { get; set; }

        public virtual CST_IPI CST_IPI { get; set; }

        public virtual CST_PISCOFINS CST_PIS { get; set; }

        public virtual CST_PISCOFINS CST_COFINS { get; set; }

        public virtual ICollection<TributacaoOperacao> TributacaoOperacao { get; set; }

    }
}