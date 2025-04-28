using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObterUsuarioPorIdAsync(int id);
        Task<Usuario> ObterUsuarioPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> ListarTodosUsuariosAsync();
        Task AdicionarUsuarioAsync(Usuario usuario);
        Task AtualizarUsuarioAsync(Usuario usuario);
        Task RemoverUsuarioAsync(int id);
    }
}