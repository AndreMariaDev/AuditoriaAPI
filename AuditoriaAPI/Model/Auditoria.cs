using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Model
{
    public class Auditoria
    {
        public int id { get; set; }
        public int idLoja { get; set; }
        public string Nomeloja { get; set; }
        public int idPessoa { get; set; }
        public String NomePessoa { get; set; }
        public String Obs { get; set; }
        public DateTime DataCadastro { get; set; }
        public int idUsuario { get; set; }

        public List<AuditoriaItem> AuditoriaItem { get; set; }
    }
}