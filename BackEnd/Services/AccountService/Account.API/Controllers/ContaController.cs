using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Application.DTOs;
using Account.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Account.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly IContaService _contaService;

        public ContaController(IContaService contaService)
        {
            _contaService = contaService;
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpPost("CriarConta")]
        public async Task<IActionResult> CriarConta([FromBody] ContaCreateDTO contaDTO)
        {
            try
            {
                var contaCriada = await _contaService.CriarContaAsync(contaDTO);
                return CreatedAtAction(nameof(CriarConta), new { id = contaCriada.Id }, contaCriada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar conta: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpGet("ListarTodasContas")]
        public async Task<IActionResult> ListarTodasContas()
        {
            try
            {
                var contas = await _contaService.ObterTodasContasAsync();
                return Ok(contas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpGet("ObterContaPorId")]
        public async Task<IActionResult> ObterContaPorId([FromQuery] int id)
        {
            try
            {
                var conta = await _contaService.ObterContaPorIdAsync(id);
                if (conta == null)
                {
                    return NotFound(new { mensagem = "Conta não encontrada." });
                }
                return Ok(conta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpPut("AtualizarConta/")]
        public async Task<IActionResult> AtualizarConta([FromQuery] int id, [FromBody] ContaUpdateDTO contaDTO)
        {
            try
            {
                if (contaDTO == null)
                {
                    return BadRequest("Dados da conta inválidos.");
                }
                var contaAtualizada = await _contaService.AtualizarContaAsync(id, contaDTO);
                if (contaAtualizada == null)
                {
                    return NotFound(new { mensagem = "Conta não encontrada." });
                }
                return Ok(contaAtualizada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpDelete("DeletarConta/")]
        public async Task<IActionResult> DeletarConta([FromQuery] int id)
        {
            try
            {
                var resultado = await _contaService.DeletarContaAsync(id);
                if (!resultado)
                {
                    return NotFound(new { mensagem = "Conta não encontrada." });
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