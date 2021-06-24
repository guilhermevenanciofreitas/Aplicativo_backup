using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("NotaFiscalModelo")]
    public partial class NotaFiscalModelo : _Extends
    {

        [Key]
        [StringLength(3)]
        public string Codigo { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

    }
}