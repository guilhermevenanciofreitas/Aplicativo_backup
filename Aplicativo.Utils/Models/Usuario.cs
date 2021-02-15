using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Model
{

    [Serializable()]
    [Table("Usuario")]
    public partial class Usuario
    {

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