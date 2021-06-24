using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("CEST")]
    public partial class CEST : _Extends
    {

        [Key]
        [StringLength(9)]
        public string Codigo { get; set; }

        [StringLength(800)]
        public string Descricao { get; set; }

    }
}