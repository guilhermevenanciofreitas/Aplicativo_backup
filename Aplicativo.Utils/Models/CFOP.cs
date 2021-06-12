using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("CFOP")]
    public partial class CFOP : _Extends
    {

        [Key]
        public int? Codigo { get; set; }

        public int? CFOPTipoID { get; set; }

        public string Descricao { get; set; }

    }
}