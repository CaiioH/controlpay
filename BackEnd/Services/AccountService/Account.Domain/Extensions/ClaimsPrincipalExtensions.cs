using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Account.Domain.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int ObterUserIdByTokenJWT(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            var idClaim = user.FindFirst(claim => claim.Type == JwtRegisteredClaimNames.NameId || claim.Type == ClaimTypes.NameIdentifier);
            Console.WriteLine($"Id do usuário: {idClaim}");
            Console.WriteLine($"Id do usuário.value: {idClaim?.Value}");
            if (idClaim == null)
            {
                throw new UnauthorizedAccessException("Id do Usuario não encontrado.");
            }
            return int.Parse(idClaim.Value);   
        }
    }
}