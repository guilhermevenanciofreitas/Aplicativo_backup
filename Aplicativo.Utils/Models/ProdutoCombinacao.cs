using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("ProdutoCombinacao")]
    public partial class ProdutoCombinacao : _Extends
    {

        public ProdutoCombinacao()
        {
            ProdutoCombinacaoAtributo = new HashSet<ProdutoCombinacaoAtributo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProdutoCombinacaoID { get; set; }

        [NotMapped]
        public int NumItem { get; set; }

        [ForeignKey("Produto")]
        public int ProdutoID { get; set; }

        [StringLength(80)]
        public string Descricao { get; set; }




        [NotMapped]
        public decimal? Saldo => EstoqueMovimentoItem?.Sum(c => c.EstoqueMovimentoItemEntrada.Saldo ?? 0) ?? 0;


        [StringLength(30)]
        public string EAN { get; set; }

        public virtual Produto Produto { get; set; }


        public virtual ICollection<ProdutoCombinacaoAtributo> ProdutoCombinacaoAtributo { get; set; }

        public virtual ICollection<EstoqueMovimentoItem> EstoqueMovimentoItem { get; set; }


    }
}