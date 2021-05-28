using Aplicativo.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Endereco")]
    public partial class Endereco : _Extends
    {

        public Endereco()
        {
            PessoaEndereco = new HashSet<PessoaEndereco>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EnderecoID { get; set; }

        [ForeignKey("EnderecoTipo")]
        public int? EnderecoTipoID { get; set; }

        [StringLength(8)]
        public string CEP { get; set; }

        [NotMapped]
        public string CEP_Formatado => CEP?.StringFormat("##.###-###");

        [StringLength(140)]
        public string Logradouro { get; set; }

        [StringLength(40)]
        public string Numero { get; set; }

        [StringLength(140)]
        public string Complemento { get; set; }

        [StringLength(100)]
        public string Bairro { get; set; }

        [NotMapped]
        public string EnderecoCompleto
        {
            get
            {
                return Logradouro.Juntar(Numero, ",").Juntar(Complemento, ",").Juntar(Bairro, " -");
            }
        }

        public int? MunicipioID { get; set; }


        public virtual EnderecoTipo EnderecoTipo { get; set; }

        public virtual Municipio Municipio { get; set; }


        public virtual ICollection<PessoaEndereco> PessoaEndereco { get; set; }


    }
}