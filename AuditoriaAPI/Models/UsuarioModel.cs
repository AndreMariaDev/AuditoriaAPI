using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Senha { get; set; }

        public string Email { get; set; }

        public string TokenSecret { get; set; }
    }
}