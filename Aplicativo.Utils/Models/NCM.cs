using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("NCM")]
    public partial class NCM : _Extends
    {

        [Key]
        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(800)]
        public string Descricao { get; set; }

    }
}