using Aplicacao.Services;
using Domino.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteService _clienteService;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ClienteService clienteService, ILogger<ClienteController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        public IActionResult Cadastro()
        {
            var model = new Cliente();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _clienteService.AdicionarClienteAsync(cliente);
                    _logger.LogInformation("Cliente salvo com sucesso.");
                    TempData["SuccessMessage"] = "Cliente salvo com sucesso!";
                    return RedirectToAction("Consulta");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao salvar o cliente.");
                    ModelState.AddModelError("", "Erro ao salvar o cliente. Tente novamente.");
                }
            }
            return View(cliente);
        }

        public async Task<IActionResult> Consulta()
        {
            var clientes = await _clienteService.ObterTodosClientesAsync();
            return View(clientes);
        }

        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Pesquisa(string cpf, string nome, string sexo, string estado, string cidade, DateTime? dataNascimento)
        {
            var clientes = await _clienteService.ObterTodosClientesAsync();

            if (!string.IsNullOrEmpty(cpf))
            {
                clientes = clientes.Where(c => c.Cpf.Contains(cpf)).ToList();
            }
            if (!string.IsNullOrEmpty(nome))
            {
                clientes = clientes.Where(c => c.Nome.Contains(nome)).ToList();
            }
            if (!string.IsNullOrEmpty(sexo))
            {
                clientes = clientes.Where(c => c.Sexo.Equals(sexo, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(estado) && estado != "Todos")
            {
                clientes = clientes.Where(c => c.Estado == estado).ToList();
            }
            if (!string.IsNullOrEmpty(cidade) && cidade != "Todos")
            {
                clientes = clientes.Where(c => c.Cidade == cidade).ToList();
            }
            if (dataNascimento.HasValue)
            {
                clientes = clientes.Where(c => c.DataNascimento.Date == dataNascimento.Value.Date).ToList();
            }

            return PartialView("_ClienteGridPartial", clientes);
        }

        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _clienteService.ObterClientePorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Json(new
            {
                id = cliente.Id,
                nome = cliente.Nome,
                cpf = cliente.Cpf,
                dataNascimento = cliente.DataNascimento.ToString("yyyy-MM-dd"), // Formato compatível com input date
                estado = cliente.Estado,
                cidade = cliente.Cidade,
                sexo = cliente.Sexo
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                _logger.LogError("Cliente é null.");
                return Json(new { success = false, message = "Dados do cliente não fornecidos." });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState é inválido.");
                // Adicionando detalhes sobre os erros de validação
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    _logger.LogError(error);
                }
                return Json(new { success = false, message = "Dados do cliente inválidos.", errors });
            }

            try
            {
                await _clienteService.AtualizarClienteAsync(cliente);
                _logger.LogInformation("Cliente atualizado com sucesso.");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o cliente.");
                return Json(new { success = false, message = "Erro ao atualizar o cliente. " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.ExcluirClienteAsync(id);
                _logger.LogInformation("Cliente removido com sucesso.");
                TempData["SuccessMessage"] = "Cliente removido com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover o cliente.");
                TempData["ErrorMessage"] = "Erro ao remover o cliente. Tente novamente.";
            }
            return RedirectToAction("Consulta");
        }
    }
}
