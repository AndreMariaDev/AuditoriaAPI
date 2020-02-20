using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Commands
{
    public class LoginUsuarioCommand
    {
        public virtual string CodigoUsuario { get; set; }
        public virtual string Senha { get; set; }
    }
}