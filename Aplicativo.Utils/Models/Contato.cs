using Aplicativo.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Contato")]
    public partial class Contato : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ContatoID { get; set; }

        [ForeignKey("ContatoTipo")]
        public int? ContatoTipoID { get; set; }

        [StringLength(60)]
        public string Nome { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        [StringLength(120)]
        public string Email { get; set; }

        public virtual ContatoTipo ContatoTipo { get; set; }

    }
}