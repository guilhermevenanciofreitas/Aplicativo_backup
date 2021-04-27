using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("Requisicao")]
    public partial class Requisicao : _Extends
    {

        public Requisicao()
        {
            RequisicaoItem = new HashSet<RequisicaoItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RequisicaoID { get; set; }
        
        //[StringLength(200)]
        //public string Nome { get; set; }

        public DateTime? DataSaida { get; set; }

        public DateTime? DataEntrada { get; set; }

        [NotMapped]
        public string Status
        {
            get
            {
                return DataEntrada == null ? "Aberta" : "Finalizada";
            }
        }

        [StringLength(400)]
        public string Observacao { get; set; }


        public virtual ICollection<RequisicaoItem> RequisicaoItem { get; set; }

    }
}