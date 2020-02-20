using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Model
{
    public class AuditoriaList
    {
        public int id { get; set; }
        public int idLoja { get; set; }
        public String NomeLoja { get; set; }
        public int idPessoa { get; set; }
        public String NomePessoa { get; set; }
        public String Obs { get; set; }
        public DateTime DataCadastro { get; set; }
        public int idUsuario { get; set; }
        public int idAuditoriaItem { get; set; }
        public int idSubTiposDocumentos { get; set; }
        public String Descricao { get; set; }
        public String NomeUsuario { get; set; }
    }
}