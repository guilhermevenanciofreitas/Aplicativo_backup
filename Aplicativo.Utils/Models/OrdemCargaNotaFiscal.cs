using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Utils.Models
{
    [Table("OrdemCargaNotaFiscal")]
    public partial class OrdemCargaNotaFiscal
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrdemCargaNotaFiscalID { get; set; }

        [ForeignKey("OrdemCarga")]
        public int? OrdemCargaID { get; set; }

        [ForeignKey("NotaFiscal")]
        public int? NotaFiscalID { get; set; }


        public virtual OrdemCarga OrdemCarga { get; set; }

        public virtual NotaFiscal NotaFiscal { get; set; }

    }
}