using Microsoft.EntityFrameworkCore;
using FuturoDoTrabalho.Api.Data;
using FuturoDoTrabalho.Api.Models;

namespace FuturoDoTrabalho.Api.Repositories
{
    public class DepartamentoRepository : GenericRepository<Departamento>, IDepartamentoRepository
    {
        public DepartamentoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Departamento?> GetByNomeAsync(string nome)
        {
            return await _context.Departamentos
                .FirstOrDefaultAsync(d => d.Nome == nome);
        }

        public async Task<List<Departamento>> GetAtivosAsync()
        {
            return await _context.Departamentos
                .Where(d => d.Ativo)
                .ToListAsync();
        }
    }
}
