using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Produto")]
    public partial class Produto : _Extends
    {
        public Produto()
        {
            EstoqueMovimentoItem = new HashSet<EstoqueMovimentoItem>();
            ProdutoAtributo = new HashSet<ProdutoAtributo>();
            ProdutoCombinacao = new HashSet<ProdutoCombinacao>();
            ProdutoPreco = new HashSet<ProdutoPreco>();
            ProdutoFornecedor = new HashSet<ProdutoFornecedor>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoID { get; set; }

        [StringLength(30)]
        public string Codigo { get; set; }

        [StringLength(80)]
        public string Descricao { get; set; }

        [ForeignKey("UnidadeMedida")]
        public int? UnidadeMedidaID { get; set; }

        public bool? Combinacao { get; set; } = false;


        public int? Origem { get; set; }

        [ForeignKey("NCM")]
        [StringLength(10)]
        public string Codigo_NCM { get; set; }

        [ForeignKey("CEST")]
        [StringLength(9)]
        public string Codigo_CEST { get; set; }

        [StringLength(30)]
        public string EAN { get; set; }


        


        [ForeignKey("Tributacao")]
        public int? TributacaoID { get; set; }


        public bool? Ativo { get; set; } = true;



        public virtual UnidadeMedida UnidadeMedida { get; set; }

        public virtual NCM NCM { get; set; }

        public virtual CEST CEST { get; set; }

        public virtual ICollection<EstoqueMovimentoItem> EstoqueMovimentoItem { get; set; }

        public virtual ICollection<ProdutoAtributo> ProdutoAtributo { get; set; }

        public virtual ICollection<ProdutoCombinacao> ProdutoCombinacao { get; set; }

        public virtual ICollection<ProdutoPreco> ProdutoPreco { get; set; }

        public virtual Tributacao Tributacao { get; set; }

        public virtual ICollection<ProdutoFornecedor> ProdutoFornecedor { get; set; }




    }
}