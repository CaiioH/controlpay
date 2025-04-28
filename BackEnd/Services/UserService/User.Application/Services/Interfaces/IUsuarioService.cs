using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Application.DTOs;

namespace User.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDTO> CriarUsuarioAsync(UsuarioDTO dto);
        Task<UsuarioResponseDTO> AtualizarUsuarioAsync(int id, UsuarioDTO dto);
        Task<bool> DeletarUsuarioAsync(int id);
        Task<UsuarioResponseDTO> ObterUsuarioPorIdAsync(int id);
        Task<UsuarioResponseDTO> ObterUsuarioPorEmailAsync(string email);
        Task<IEnumerable<UsuarioResponseDTO>> ListarTodosUsuariosAsync();
        Task<LoginResponseDTO> AutenticarUsuarioAsync(LoginRequestDTO dto);

    }
}