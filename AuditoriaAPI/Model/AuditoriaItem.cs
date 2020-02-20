using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Model
{
    public class AuditoriaItem
    {
        public int id { get; set; }
        public int idAuditoria { get; set; }
        public int idSubTiposDocumentos { get; set; }
        public string DocumentosDescricao { get; set; }
    }
}