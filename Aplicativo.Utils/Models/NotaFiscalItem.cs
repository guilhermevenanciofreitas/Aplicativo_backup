using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("NotaFiscalItem")]
    public partial class NotaFiscalItem : _Extends
    {

        public NotaFiscalItem()
        {
            PedidoVendaItemNotaFiscalItem = new HashSet<PedidoVendaItemNotaFiscalItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? NotaFiscalItemID { get; set; }

        [ForeignKey("NotaFiscal")]
        public int? NotaFiscalID { get; set; }

        [NotMapped]
        public int? ProdutoID { get; set; }

        [StringLength(20)]
        public string cProd { get; set; }

        [NotMapped]
        public string CodigoDescricao => cProd + " - " + xProd;

        [StringLength(30)]
        public string cEAN { get; set; }

        [StringLength(130)]
        public string xProd { get; set; }

        [StringLength(8)]
        public string Codigo_NCM { get; set; }

        [StringLength(7)]
        public string Codigo_CEST { get; set; }

        [ForeignKey("CFOP")]
        [StringLength(5)]
        public string Codigo_CFOP { get; set; }


        //Tributação

        public int? orig { get; set; }

        public string Codigo_CST { get; set; }

        public string Codigo_CSOSN { get; set; }



        public string IPI_CST { get; set; }
        public string PIS_CST { get; set; }
        public string COFINS_CST { get; set; }


        public decimal? pICMS { get; set; }
        public decimal? pIPI { get; set; }
        public decimal? pPIS { get; set; }
        public decimal? pCOFINS { get; set; }

        public decimal? vICMS { get; set; }
        public decimal? vIPI { get; set; }
        public decimal? vPIS { get; set; }
        public decimal? vCOFINS { get; set; }


        [StringLength(15)]
        public string uCom { get; set; }

        public decimal? qCom { get; set; }

        public decimal? vUnCom { get; set; }

        public decimal? vDesc { get; set; }

        public decimal? vProd { get; set; }

        public virtual NotaFiscal NotaFiscal { get; set; }


        public virtual CFOP CFOP { get; set; }

        public virtual ICollection<PedidoVendaItemNotaFiscalItem> PedidoVendaItemNotaFiscalItem { get; set; }


    }
}