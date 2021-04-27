using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("ProdutoTributacao")]
    public partial class ProdutoTributacao : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoTributacaoID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }


        public int? CFOP_Compra { get; set; }
        public int? CFOP_CompraDevolucao { get; set; }
        public int? CFOP_Venda { get; set; }
        public int? CFOP_VendaDevolucao { get; set; }


        public string ICMS_CST { get; set; }

        public string ICMS_CSOSN { get; set; }

        public string IPI_CST { get; set; }

        public string PIS_CST { get; set; }

        public string COFINS_CST { get; set; }

    }
}