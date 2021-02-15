using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Model
{

    [Serializable()]
    [Table("Municipio")]
    public partial class Municipio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? MunicipioID { get; set; }


        public int? IBGE { get; set; }

        [StringLength(255)]
        public string Nome { get; set; }

        [Required]
        [StringLength(2)]
        public string UF { get; set; }

        [NotMapped]
        public string MunicipioUF { 
            get 
            {
                return Nome + " - " + UF;
            } 
        }

        [ForeignKey("Estado")]
        public int? EstadoID { get; set; }

        public virtual Estado Estado { get; set; }


    }

}