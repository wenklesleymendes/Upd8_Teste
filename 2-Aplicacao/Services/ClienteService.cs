using Domino.Entities;
using InfraEstrutura.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacao.Services
{
    public class ClienteService
    {
        private readonly ClienteContext _context;

        public ClienteService(ClienteContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> ObterTodosClientesAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> ObterClientePorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task AdicionarClienteAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarClienteAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirClienteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
