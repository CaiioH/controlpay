using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application.DTOs;
using User.Application.Services.Interfaces;

namespace User.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
       private readonly IUsuarioService _usuarioService;

       public UsuarioController(IUsuarioService usuarioService)
       {
           _usuarioService = usuarioService;
       }

        [HttpPost("SignUp")]
        // [ProducesResponseType(typeof(UsuarioResponseDTO), 201)]
        // [ProducesResponseType(typeof(string), 400)]
        // [ProducesResponseType(typeof(string), 409)]
        // [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> CriarUsuario([FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                var usuarioCriado = await _usuarioService.CriarUsuarioAsync(usuarioDto);
                
                return CreatedAtAction(nameof(CriarUsuario), new { id = usuarioCriado.Id }, usuarioCriado);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar usuário: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ListarTodosUsuarios")]
        public async Task<IActionResult> ListarTodosUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ListarTodosUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ObterUsuarioPorId/")]
        public async Task<IActionResult> ObterUsuarioPorId([FromQuery] int id)
        {
            try
            {
                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ObterUsuarioPorEmail/")]
        public async Task<IActionResult> ObterUsuarioPorEmail([FromQuery] string email)
        {
            try
            {
                var usuario = await _usuarioService.ObterUsuarioPorEmailAsync(email);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("AtualizarUsuario/")]
        public async Task<IActionResult> AtualizarUsuario([FromQuery] int id, [FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                if (usuarioDto == null)
                {
                    return BadRequest("Dados do usuário inválidos.");
                }

                var usuarioAtualizado = await _usuarioService.AtualizarUsuarioAsync(id, usuarioDto);
                if (usuarioAtualizado == null)
                {
                    return NotFound("Usuário não encontrado.");
                }
                return Ok(usuarioAtualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletarUsuario/")]
        public async Task<IActionResult> DeletarUsuario([FromQuery] int id)
        {
            try
            {
                var usuarioDeletado = await _usuarioService.DeletarUsuarioAsync(id);
                if (!usuarioDeletado)
                {
                    return NotFound("Usuário não encontrado.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}