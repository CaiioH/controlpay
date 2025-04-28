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
    public class ParcelaController : ControllerBase
    {
        private readonly IParcelaService _parcelaService;

        public ParcelaController(IParcelaService parcelaService)
        {
            _parcelaService = parcelaService;
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpPost("CriarParcela")]
        public async Task<IActionResult> CriarParcela([FromBody] ParcelaCreateDTO parcelaDto)
        {
            try
            {
                var parcelaCriada = await _parcelaService.AdicionarParcelaAsync(parcelaDto);
                return CreatedAtAction(nameof(CriarParcela), new { id = parcelaCriada.Id }, parcelaCriada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar parcela: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpGet("ListarTodasParcelas")]
        public async Task<IActionResult> ListarTodasParcelas()
        {
            try
            {
                var parcelas = await _parcelaService.ObterTodasParcelasAsync();
                return Ok(parcelas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpGet("ObterParcelaPorId")]
        public async Task<IActionResult> ObterParcelaPorId([FromQuery] int id)
        {
            try
            {
                var parcela = await _parcelaService.ObterParcelaByIdAsync(id);
                if (parcela == null)
                {
                    return NotFound(new { mensagem = "Parcela não encontrada." });
                }
                return Ok(parcela);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpPut("AtualizarParcela")]
        public async Task<IActionResult> AtualizarParcela([FromQuery] int id, [FromBody] ParcelaCreateDTO parcelaDto)
        {
            try
            {
                if (parcelaDto == null)
                {
                    return BadRequest("Dados invalidos");
                }

                var parcelaAtualizada = await _parcelaService.AtualizarParcelaAsync(id, parcelaDto);
                if (parcelaAtualizada == null)
                {
                    return NotFound("Parcela não encontrada.");
                }
                return Ok(parcelaAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar parcela: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpDelete("DeletarParcela")]
        public async Task<IActionResult> DeletarParcela([FromQuery] int id)
        {
            try
            {
                var resultado = await _parcelaService.DeletarParcelaAsync(id);
                if (!resultado)
                {
                    return NotFound("Parcela não encontrada.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar parcela: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpGet("ObterParcelasPorContaId")]
        public async Task<IActionResult> ObterParcelasPorContaId([FromQuery] int contaId)
        {
            try
            {
                var parcelas = await _parcelaService.ObterParcelasPorContaIdAsync(contaId);
                return Ok(parcelas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}