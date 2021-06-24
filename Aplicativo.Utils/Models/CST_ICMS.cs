using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("CST_ICMS")]
    public partial class CST_ICMS : _Extends
    {

        [Key]
        [StringLength(2)]
        public string Codigo { get; set; }

        [StringLength(200)]
        public string Descricao { get; set; }

    }
}