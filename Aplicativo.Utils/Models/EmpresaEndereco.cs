﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Model
{

    [Serializable()]
    [Table("EmpresaEndereco")]
    public partial class EmpresaEndereco
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EmpresaEnderecoID { get; set; }

        [ForeignKey("Empresa")]
        public int? EmpresaID { get; set; }

        [ForeignKey("Endereco")]
        public int? EnderecoID { get; set; }


        public virtual Empresa Empresa { get; set; }

        public virtual Endereco Endereco { get; set; }

    }
}