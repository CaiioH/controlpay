using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.DTOs;
using Payment.Application.Interfaces;

namespace Payment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        public PaymentController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpPost("RegistrarPagamento")]
        public async Task<IActionResult> RegistrarPagamento([FromBody] PagamentoCreateDTO pagamentoCreateDTO)
        {
            try
            {
                var pagamento = await _pagamentoService.RegistrarPagamentoAsync(pagamentoCreateDTO);
                return CreatedAtAction(nameof(RegistrarPagamento), new { id = pagamento.Id }, pagamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao registrar pagamento: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpPut("AtualizarPagamento")]
        public async Task<IActionResult> AtualizarPagamento([FromQuery] int id, [FromBody] PagamentoCreateDTO pagamentoUpdateDTO)
        {
            try
            {
                var pagamentoAtualizado = await _pagamentoService.AtualizarPagamentoAsync(id, pagamentoUpdateDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar pagamento: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpGet("ListarPagamentosPorParcelaId")]
        public async Task<IActionResult> ListarPagamentosPorParcelaId([FromQuery] int parcelaId)
        {
            try
            {
                var pagamentos = await _pagamentoService.ListarPagamentosPorParcelaIdAsync(parcelaId);
                if (pagamentos == null || !pagamentos.Any())
                    return NotFound(new { mensagem = "Nenhum pagamento encontrado para a parcela informada." });
                return Ok(pagamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar pagamentos: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpGet("ObterPagamentoPorId")]
        public async Task<IActionResult> ObterPagamentoPorId([FromQuery] int id)
        {
            try
            {
                var pagamento = await _pagamentoService.ObterPagamentoPorIdAsync(id);
                if (pagamento == null)
                    return NotFound(new { mensagem = "Pagamento não encontrado." });
                return Ok(pagamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter pagamento: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpDelete("RemoverPagamento")]
        public async Task<IActionResult> RemoverPagamento([FromQuery] int id)
        {
            try
            {
                var resultado = await _pagamentoService.RemoverPagamentoAsync(id);
                if (!resultado)
                    return NotFound(new { mensagem = "Pagamento não encontrado." });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover pagamento: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,UsuarioRaizComum")]
        [HttpGet("ListarTodosPagamentos")]
        public async Task<IActionResult> ListarTodosPagamentos()
        {
            try
            {
                var pagamentos = await _pagamentoService.ListarTodosPagamentosAsync();
                if (pagamentos == null || !pagamentos.Any())
                    return NotFound(new { mensagem = "Nenhum pagamento encontrado." });

                return Ok(pagamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar pagamentos: {ex.Message}");
            }
        }

    }
}