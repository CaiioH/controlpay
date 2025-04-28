using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Domain.Utilities;

namespace User.Application.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int NivelPermissao { get; set; }
    }
}