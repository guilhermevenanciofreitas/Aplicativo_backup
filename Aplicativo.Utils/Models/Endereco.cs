using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Model
{

    [Serializable()]
    [Table("Endereco")]
    public partial class Endereco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EnderecoID { get; set; }

        [ForeignKey("EnderecoTipo")]
        public int? EnderecoTipoID { get; set; }

        [StringLength(8)]
        public string CEP { get; set; }

        [StringLength(140)]
        public string Logradouro { get; set; }

        [StringLength(40)]
        public string Numero { get; set; }

        [NotMapped]
        public string LogradouroNumeroComplemento
        { 
            get 
            {
                return Logradouro + ", " + Numero + ", " + Complemento;
            } 
        }

        [StringLength(140)]
        public string Complemento { get; set; }

        [StringLength(100)]
        public string Bairro { get; set; }

        public int? MunicipioID { get; set; }


        public virtual EnderecoTipo EnderecoTipo { get; set; }

        public virtual Municipio Municipio { get; set; }

    }
}