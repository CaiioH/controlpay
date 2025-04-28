using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Application.DTOs
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}