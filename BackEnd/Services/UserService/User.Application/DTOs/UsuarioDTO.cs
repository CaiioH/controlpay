using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Domain.Utilities;

namespace User.Application.DTOs
{
    public class UsuarioDTO
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public NivelPermissao NivelPermissao { get; set; } // Enum

    }
}