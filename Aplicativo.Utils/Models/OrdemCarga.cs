using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("OrdemCarga")]
    public partial class OrdemCarga
    {
        public OrdemCarga()
        {
            OrdemCargaAndamento = new HashSet<OrdemCargaAndamento>();
            OrdemCargaNotaFiscal = new HashSet<OrdemCargaNotaFiscal>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrdemCargaID { get; set; }


        [ForeignKey("OrdemCargaStatus")]
        public int? OrdemCargaStatusID { get; set; }


        [ForeignKey("Remetente")]
        public int? RemetenteID { get; set; }


        [ForeignKey("Motorista")]
        public int? MotoristaID { get; set; }

        //[ForeignKey("VeiculoColeta")]
        //public int? VeiculoColetaID { get; set; }

        //[ForeignKey("CarretaColeta1")]
        //public int? CarretaColeta1ID { get; set; }

        //[ForeignKey("CarretaColeta2")]
        //public int? CarretaColeta2ID { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }



        public DateTime? DataCarregamento { get; set; }

        public DateTime? DataLimiteSaida { get; set; }


        public virtual OrdemCargaStatus OrdemCargaStatus { get; set; }

        public virtual Pessoa Remetente { get; set; }

        public virtual Usuario Motorista { get; set; }

        //public virtual Veiculo VeiculoColeta { get; set; }

        //public virtual Veiculo CarretaColeta1 { get; set; }

        //public virtual Veiculo CarretaColeta2 { get; set; }

        public virtual ICollection<OrdemCargaAndamento> OrdemCargaAndamento { get; set; }

        public virtual ICollection<OrdemCargaNotaFiscal> OrdemCargaNotaFiscal { get; set; }

    }
}