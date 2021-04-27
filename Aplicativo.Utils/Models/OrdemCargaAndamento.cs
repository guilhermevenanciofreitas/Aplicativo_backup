using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("OrdemCargaAndamento")]
    public partial class OrdemCargaAndamento
    {
 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrdemCargaAndamentoID { get; set; }

        [ForeignKey("OrdemCarga")]
        public int? OrdemCargaID { get; set; }

        [ForeignKey("OrdemCargaStatus")]
        public int? OrdemCargaStatusID { get; set; }

        public DateTime? Data { get; set; }

        [StringLength(100)]
        public string Observacao { get; set; }


        public virtual OrdemCarga OrdemCarga { get; set; }

        public virtual OrdemCargaStatus OrdemCargaStatus { get; set; }

    }
}