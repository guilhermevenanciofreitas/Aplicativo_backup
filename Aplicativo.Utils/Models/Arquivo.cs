using Aplicativo.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Aplicativo.Utils.Models
{

    [Serializable()]
    [Table("Arquivo")]
    public partial class Arquivo : _Extends
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ArquivoID { get; set; }

        [ForeignKey("ArquivoTipo")]
        public int? ArquivoTipoID { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(100)]
        public string Observacao { get; set; }

        public byte[] Anexo { get; set; }

        public long? Tamanho { get; set; }

        [NotMapped]
        public string Bytes
        {
            get
            {
                if (Tamanho == null || Tamanho == 0)
                    return "";
                else
                    return Tamanho.Value.ToString();
            }
        }

        [NotMapped]
        public string KiloBytes
        {
            get
            {
                if (Tamanho == null || Tamanho == 0)
                    return "";
                else
                    return (Math.Round(Tamanho.Value / 1024f, 2)).ToString();
            }
        }

        [NotMapped]
        public string MegaBytes { 
            get {
                if (Tamanho == null || Tamanho == 0)
                    return "";
                else
                    return (Math.Round((Tamanho.Value / 1024f) / 1024f, 2)).ToString();
            } 
        }

        [NotMapped]
        public string GigaBytes
        {
            get
            {
                if (Tamanho == null || Tamanho == 0)
                    return "";
                else
                    return (Math.Round(((Tamanho.Value / 1024f) / 1024f) / 1024f, 2)).ToString();
            }
        }

        public virtual ArquivoTipo ArquivoTipo { get; set; }

    }

}