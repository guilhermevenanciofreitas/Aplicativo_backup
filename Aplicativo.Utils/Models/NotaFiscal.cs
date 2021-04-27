using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("NotaFiscal")]
    public partial class NotaFiscal : _Extends
    {

        public NotaFiscal()
        {
            NotaFiscalItem = new HashSet<NotaFiscalItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? NotaFiscalID { get; set; }

        public byte? tpNF { get; set; } = 0;

        public int? tpEmis { get; set; }

        public byte? tpAmb { get; set; } = 1;

        public byte? finNFe { get; set; } = 1;

        [StringLength(200)]
        public string natOp { get; set; }

        [StringLength(8)]
        public string cNF { get; set; }


        public int? nNF { get; set; }

        public int? serie { get; set; }

        [StringLength(8)]
        public string mod { get; set; } = "55";

        public byte? indPres { get; set; }

        [StringLength(44)]
        public string chNFe { get; set; }


        [NotMapped]
        public int? _serie => Convert.ToInt32(chNFe.Substring(23, 2));

        [NotMapped]
        public long? _nNF => Convert.ToInt64(chNFe.Substring(26, 8));


        public byte? nSeqEvento { get; set; }

        [StringLength(15)]
        public string nProt { get; set; }

        public int? cStat { get; set; }

        [StringLength(200)]
        public string xMotivo { get; set; }

        [NotMapped]
        public string Status => cStat + " - " + xMotivo;

        public DateTime? dhEmi { get; set; } = DateTime.Now;

        public DateTime? dhSaiEnt { get; set; } = DateTime.Now;

        [StringLength(14)]
        public string CNPJCPF { get; set; }

        [StringLength(100)]
        public string xNome { get; set; }

        [StringLength(100)]
        public string xFant { get; set; }

        [StringLength(15)]
        public string IE { get; set; }

        public int? CRT { get; set; }

        [StringLength(140)]
        public string xLgr { get; set; }

        [StringLength(40)]
        public string nro { get; set; }

        [StringLength(140)]
        public string xCpl { get; set; }

        [StringLength(100)]
        public string xBairro { get; set; }

        public int? cMun { get; set; }

        [StringLength(255)]
        public string xMun { get; set; }

        [StringLength(2)]
        public string UF { get; set; }

        [StringLength(8)]
        public string CEP { get; set; }




        [StringLength(14)]
        public string dest_CNPJCPF { get; set; }

        [StringLength(15)]
        public string dest_IE { get; set; }

        [StringLength(100)]
        public string dest_xNome { get; set; }

        //[StringLength(15)]
        //public string IE { get; set; }

        [StringLength(140)]
        public string dest_xLgr { get; set; }

        [StringLength(40)]
        public string dest_nro { get; set; }

        [StringLength(140)]
        public string dest_xCpl { get; set; }

        [StringLength(100)]
        public string dest_xBairro { get; set; }

        public int? dest_cMun { get; set; }

        [StringLength(255)]
        public string dest_xMun { get; set; }

        [StringLength(2)]
        public string dest_UF { get; set; }

        [StringLength(8)]
        public string dest_CEP { get; set; }






        [StringLength(14)]
        public string transp_CNPJCPF { get; set; }

        [StringLength(15)]
        public string transp_IE { get; set; }

        [StringLength(100)]
        public string transp_xNome { get; set; }


        [StringLength(200)]
        public string transp_xEnder { get; set; }

        //[StringLength(15)]
        //public string IE { get; set; }

        //[StringLength(140)]
        //public string transp_xLgr { get; set; }

        //[StringLength(40)]
        //public string transp_nro { get; set; }

        //[StringLength(140)]
        //public string transp_xCpl { get; set; }

        //[StringLength(100)]
        //public string transp_xBairro { get; set; }

        public int? transp_cMun { get; set; }

        [StringLength(255)]
        public string transp_xMun { get; set; }

        [StringLength(2)]
        public string transp_UF { get; set; }

        [StringLength(8)]
        public string transp_CEP { get; set; }






        [StringLength(5000)]
        public string infCpl { get; set; }

        [StringLength(16)]
        public string Fone { get; set; }

        public decimal? vBC { get; set; } = 0;

        public decimal? vICMS { get; set; } = 0;

        public decimal? vBCST { get; set; } = 0;

        public decimal? vST { get; set; } = 0;

        public decimal? vProd { get; set; } = 0;

        public decimal? vFrete { get; set; } = 0;

        public decimal? vSeg { get; set; } = 0;

        public decimal? vDesc { get; set; } = 0;

        public decimal? vII { get; set; } = 0;

        public decimal? vIPI { get; set; } = 0;

        public decimal? vPIS { get; set; } = 0;

        public decimal? vCOFINS { get; set; } = 0;

        public decimal? vOutro { get; set; } = 0;

        public decimal? vNF { get; set; } = 0;

        public int? modFrete { get; set; } = 9;

        //public DateTime? DataFinalizado { get; set; }

        //public DateTime? DataCancelamento { get; set; }

        //public int? EventoCancelamentoID { get; set; }

        //public bool Emitida { get; set; } = false;

        [ForeignKey("XmlArquivo")]
        public int? XmlArquivoID { get; set; }

        //[NotMapped]
        //public decimal? vTotal
        //{
        //    get
        //    {
        //        decimal? _vTotal = 0;
        //        if (NotaFiscalItem != null)
        //        {
        //            foreach (var Item in NotaFiscalItem.ToList())
        //                _vTotal += Item.vProd;
        //        }
        //        return _vTotal;
        //    }
        //}

        //[NotMapped]
        //public string Situacao
        //{
        //    get
        //    {
        //        if (DataCancelamento != null)
        //        {
        //            return "Cancelado";
        //        }
        //        else if (DataFinalizado != null)
        //        {
        //            return "Enviado Sefaz";
        //        }
        //        else
        //        {
        //            return "Aberta";
        //        }
        //    }
        //}

        //public virtual Parceiro Parceiro { get; set; }

        //public virtual NotaFiscalModelo NotaFiscalModelo { get; set; }

        //public virtual ICollection<NotaFiscalItem> NotaFiscalItem { get; set; }

        //public virtual ICollection<NotaFiscalPagamento> NotaFiscalPagamento { get; set; }

        //public virtual ICollection<NotaFiscalEvento> NotaFiscalEvento { get; set; }

        //public virtual NotaFiscalEvento EventoCancelamento { get; set; }

        public virtual Arquivo XmlArquivo { get; set; }

        public virtual ICollection<NotaFiscalItem> NotaFiscalItem { get; set; }

        public virtual ICollection<OrdemCargaNotaFiscal> OrdemCargaNotaFiscal { get; set; }

    }
}