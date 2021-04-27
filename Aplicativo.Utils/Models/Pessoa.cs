using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    public enum TipoPessoa
    {
        Fisica = 1,
        Juridica = 2,
    }

    public enum Sexo
    {
        Masculino = 1,
        Feminino = 2,
    }

    [Serializable()]
    [Table("Pessoa")]
    public partial class Pessoa : _Extends
    {

        public Pessoa()
        {

            Vendedor = new HashSet<PessoaVendedor>();
            Clientes = new HashSet<PessoaVendedor>();

            Usuario = new HashSet<Usuario>();

            PessoaEndereco = new HashSet<PessoaEndereco>();
            PessoaContato = new HashSet<PessoaContato>();
            PessoaArquivo = new HashSet<PessoaArquivo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PessoaID { get; set; }

        public TipoPessoa? TipoPessoaID { get; set; }

        [StringLength(14, ErrorMessage = "CPF/CNPJ - tamanho máximo 14 caracteres")]
        public string CNPJ { get; set; }

        [StringLength(100)]
        public string NomeFantasia { get; set; }

        [StringLength(140)]
        public string RazaoSocial { get; set; }

        public bool? IsCliente { get; set; }

        public bool? IsLead { get; set; }

        public bool? IsFornecedor { get; set; }

        public bool? IsTransportadora { get; set; }

        public bool? IsFuncionario { get; set; }

        [StringLength(30)]
        public string IE { get; set; }

        [StringLength(30)]
        public string IM { get; set; }

        public Sexo? Sexo { get; set; }



        [ForeignKey("Preco")]
        public int? PrecoID { get; set; }

        [ForeignKey("PessoaPerfil")]
        public int? PessoaPerfilID { get; set; }

        [ForeignKey("PessoaSegmento")]
        public int? PessoaSegmentoID { get; set; }

        public DateTime? Abertura { get; set; }

        [StringLength(300)]
        public string Observacao { get; set; }

        public bool? Ativo { get; set; } = true;


        public virtual ICollection<PessoaVendedor> Vendedor { get; set; }

        public virtual ICollection<PessoaVendedor> Clientes { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }

        public virtual Preco Preco { get; set; }

        public virtual PessoaPerfil PessoaPerfil { get; set; }

        public virtual PessoaSegmento PessoaSegmento { get; set; }

        public virtual ICollection<PessoaEndereco> PessoaEndereco { get; set; }

        public virtual ICollection<PessoaContato> PessoaContato { get; set; }

        public virtual ICollection<PessoaArquivo> PessoaArquivo { get; set; }

    }
}