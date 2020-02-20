using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Models
{
    public class AuditoriaModel
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int IdPessoa { get; set; }
        public String Obs { get; set; }
        public string AuditoriaItem { get; set; }
        public string TokenSecret { get; set; }
        public int IdUsuario { get; set; }
    }
}