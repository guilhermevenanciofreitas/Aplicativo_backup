using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("CST_IPI")]
    public partial class CST_IPI : _Extends
    {

        [Key]
        [StringLength(2)]
        public string Codigo { get; set; }

        [StringLength(200)]
        public string Descricao { get; set; }

    }
}