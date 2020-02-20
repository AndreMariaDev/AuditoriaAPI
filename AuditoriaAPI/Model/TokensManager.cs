using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Model
{
    public class TokensManager
    {
        public int TokenID { get; set; }
        public String TokenKey { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public int idUsuario { get; set; }
        public bool Deleted { get; set; }
    }
}