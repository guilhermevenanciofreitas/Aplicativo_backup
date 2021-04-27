using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("AgendaTipo")]
    public partial class AgendaTipo : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? AgendaTipoID { get; set; }


        [StringLength(60)]
        public string Descricao { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

    }
}