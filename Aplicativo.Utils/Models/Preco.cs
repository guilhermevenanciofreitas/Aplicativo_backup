using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Preco")]
    public partial class Preco : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PrecoID { get; set; }

        [StringLength(80)]
        public string Descricao { get; set; }

        public bool? Ativo { get; set; }


    }
}