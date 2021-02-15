using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Model
{

    [Serializable()]
    [Table("Empresa")]
    public partial class Empresa
    {

        public Empresa()
        {
            EmpresaEndereco = new HashSet<EmpresaEndereco>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EmpresaID { get; set; }

        public int? TipoPessoaID { get; set; } = 1;

        [StringLength(14, ErrorMessage = "CPF/CNPJ - tamanho máximo 14 caracteres")]
        public string CNPJ { get; set; }

        [StringLength(15)]
        public string IE { get; set; }

        [StringLength(100)]
        public string NomeFantasia { get; set; }

        [StringLength(140)]
        public string RazaoSocial { get; set; }

        public int? CRT { get; set; }

        public long? ultNSU { get; set; }


        public int? NFe_Numero { get; set; }

        public int? NFe_Serie { get; set; }


        public bool? Ativo { get; set; } = true;


        public virtual ICollection<EmpresaEndereco> EmpresaEndereco { get; set; }

    }
}