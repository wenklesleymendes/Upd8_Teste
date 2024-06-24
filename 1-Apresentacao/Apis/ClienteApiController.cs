using Aplicacao.Services;
using Domino.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteApiController : ControllerBase
    {
        private readonly ClienteService _clienteService;
        private readonly ILogger<ClienteApiController> _logger;

        public ClienteApiController(ClienteService clienteService, ILogger<ClienteApiController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                _logger.LogError("Cliente é null.");
                return BadRequest("Dados do cliente não fornecidos.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState é inválido.");
                return BadRequest(ModelState);
            }

            try
            {
                await _clienteService.AdicionarClienteAsync(cliente);
                _logger.LogInformation("Cliente salvo com sucesso.");
                return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar o cliente.");
                return StatusCode(500, "Erro ao salvar o cliente. Tente novamente.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.ObterClientePorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("ID do cliente não corresponde.");
            }

            if (cliente == null)
            {
                _logger.LogError("Cliente é null.");
                return BadRequest("Dados do cliente não fornecidos.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState é inválido.");
                return BadRequest(ModelState);
            }

            try
            {
                await _clienteService.AtualizarClienteAsync(cliente);
                _logger.LogInformation("Cliente atualizado com sucesso.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o cliente.");
                return StatusCode(500, "Erro ao atualizar o cliente. Tente novamente.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.ExcluirClienteAsync(id);
                _logger.LogInformation("Cliente removido com sucesso.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover o cliente.");
                return StatusCode(500, "Erro ao remover o cliente. Tente novamente.");
            }
        }
    }
}
