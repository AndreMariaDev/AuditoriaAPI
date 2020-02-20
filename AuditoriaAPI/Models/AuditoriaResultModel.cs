using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Models
{
    public class AuditoriaResultModel
    {
        public String NomeLoja { get; set; }
        public String NomeColaborador { get; set; }
        public String Obs { get; set; }
        public String Documentos { get; set; }
        public String DataCadastro { get; set; }
        public int Index { get; set; }
    }
}