using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Model
{
    public class SubTiposDocumentos
    {
        public int IdSubTipoDocumento { get; set; }
        public int IdTipoDocumento { get; set; }
        public String Descricao { get; set; }
    }
}