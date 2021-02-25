using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Usuario")]
    public partial class Usuario : _Extends
    {

        public Usuario()
        {
            UsuarioEmail = new HashSet<UsuarioEmail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UsuarioID { get; set; }
        
        [StringLength(30)]
        public string Login { get; set; }

        [StringLength(40)]
        public string Senha { get; set; }

        [StringLength(30)]
        public string Nome { get; set; }

        public bool Ativo { get; set; } = true;


        public ICollection<UsuarioEmail> UsuarioEmail { get; set; }

    }
}