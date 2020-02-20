using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Model
{
    public class ClientKey
    {
        public int ClientKeyId { get; set; }
        public String ClientID { get; set; }
        public String ClientSecret { get; set; }
    }
}