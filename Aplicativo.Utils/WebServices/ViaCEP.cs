using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicativo.Utils.WebServices
{
    public class ViaCEP
    {

        public bool erro { get; set; }

        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string municipio { get; set; }

        public int? MunicipioID { get; set; }
        public int? EstadoID { get; set; }
        public string UF { get; set; }

        public string unidade { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }

    }
}
