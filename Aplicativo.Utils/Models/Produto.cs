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

        [StringLength(8)]
        public string NCM { get; set; }

        [StringLength(7)]
        public string CEST { get; set; }

        [StringLength(30)]
        public string EAN { get; set; }


        


        [ForeignKey("Tributacao")]
        public int? TributacaoID { get; set; }


        public bool? Ativo { get; set; } = true;



        public virtual UnidadeMedida UnidadeMedida { get; set; }

        public virtual ICollection<EstoqueMovimentoItem> EstoqueMovimentoItem { get; set; }

        public virtual ICollection<ProdutoAtributo> ProdutoAtributo { get; set; }

        public virtual ICollection<ProdutoCombinacao> ProdutoCombinacao { get; set; }

        public virtual ICollection<ProdutoPreco> ProdutoPreco { get; set; }

        public virtual Tributacao Tributacao { get; set; }

        public virtual ICollection<ProdutoFornecedor> ProdutoFornecedor { get; set; }




    }
}