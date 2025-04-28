using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Application.DTOs;
using User.Application.Helpers;
using User.Application.Services.Interfaces;
using User.Domain.Interfaces;
using User.Domain.Models;

namespace User.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<UsuarioResponseDTO> CriarUsuarioAsync(UsuarioDTO dto)
        {
            try
            {
                var existente = await _usuarioRepository.ObterUsuarioPorEmailAsync(dto.Email);
                if (existente != null)
                    throw new Exception("E-mail já está em uso.");

                Console.WriteLine($"Senha do usuario na service: {dto.Senha}");
                var usuario = new Usuario(dto.Nome, dto.Email, dto.Senha, dto.NivelPermissao);
                Console.WriteLine($"Senha do usuario na service: {usuario.Senha}");
                await _usuarioRepository.AdicionarUsuarioAsync(usuario);
                return new UsuarioResponseDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioResponseDTO> AtualizarUsuarioAsync(int id, UsuarioDTO dto)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                    throw new Exception("Usuário não encontrado.");

                // ta faltando validação aqui ainda pra permitir que o usuario atualize apenas seu proprio cadastro
                // e não o de outros usuarios, ou seja, o id do usuario logado tem que ser o mesmo do id do usuario que ele quer atualizar
                // isso pode ser feito com o token, pegando o id do usuario logado e comparando com o id do usuario que ele quer atualizar
                // se não for o mesmo, retorna um erro de acesso negado ou algo do tipo
                // E também poder atualizar apenas o nome e o email, não a senha, a senha só pode ser atualizada pelo próprio usuario logado
                // ou pelo admin, mas o admin não pode atualizar a senha de outros usuarios, apenas o proprio usuario logado pode fazer isso
                // e permitir atualizar apenas o atributo que quiser.

                usuario.AtualizarDados(dto.Nome, dto.Email);
                await _usuarioRepository.AtualizarUsuarioAsync(usuario);

                return new UsuarioResponseDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeletarUsuarioAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                    throw new Exception("Usuário não encontrado.");

                await _usuarioRepository.RemoverUsuarioAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioResponseDTO> ObterUsuarioPorIdAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                    throw new Exception("Usuário não encontrado.");

                return new UsuarioResponseDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UsuarioResponseDTO> ObterUsuarioPorEmailAsync(string email)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterUsuarioPorEmailAsync(email);

                return new UsuarioResponseDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<UsuarioResponseDTO>> ListarTodosUsuariosAsync()
        {
            try
            {
                var usuarios = await _usuarioRepository.ListarTodosUsuariosAsync();
                return usuarios.Select(u => new UsuarioResponseDTO
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoginResponseDTO> AutenticarUsuarioAsync(LoginRequestDTO dto)
        {
            var usuario = await _usuarioRepository.ObterUsuarioPorEmailAsync(dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha))
                throw new Exception("E-mail ou senha inválidos");

            var token = _tokenService.GerarToken(usuario);

            return new LoginResponseDTO
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                NivelPermissao = (int)usuario.NivelPermissao,
                Token = token
            };
        }

    }
}