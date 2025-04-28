using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Application.Helpers
{
    public interface ITokenService
    {
        public string GerarToken(Usuario usuario);
    }
}