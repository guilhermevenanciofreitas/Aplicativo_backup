using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("EstoqueMovimentoItem")]
    public partial class EstoqueMovimentoItem : _Extends
    {

        public EstoqueMovimentoItem()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EstoqueMovimentoItemID { get; set; }

        [ForeignKey("EstoqueMovimento")]
        public int? EstoqueMovimentoID { get; set; }

        [ForeignKey("Produto")]
        public int? ProdutoID { get; set; }

        [ForeignKey("ProdutoCombinacao")]
        public int? ProdutoCombinacaoID { get; set; }

        [ForeignKey("NotaFiscalItem")]
        public int? NotaFiscalItemID { get; set; }

        public decimal? Quantidade { get; set; }

        [NotMapped]
        public decimal? Preco { get; set; }

        [ForeignKey("EstoqueMovimentoItemEntrada")]
        public int? EstoqueMovimentoItemEntradaID { get; set; }

        [ForeignKey("EstoqueMovimentoItemSaida")]
        public int? EstoqueMovimentoItemSaidaID { get; set; }

        [NotMapped]
        public string ProdutoVinculado => ProdutoID + " - " + Produto?.Descricao;

        [NotMapped]
        public string ItemNF => NotaFiscalItem?.cProd + " - " + NotaFiscalItem?.xProd;

        public virtual EstoqueMovimento EstoqueMovimento { get; set; }

        public virtual Produto Produto { get; set; }

        public virtual ProdutoCombinacao ProdutoCombinacao { get; set; }

        public virtual NotaFiscalItem NotaFiscalItem { get; set; }

        public virtual EstoqueMovimentoItemEntrada EstoqueMovimentoItemEntrada { get; set; }

        public virtual EstoqueMovimentoItemSaida EstoqueMovimentoItemSaida { get; set; }

    }
}