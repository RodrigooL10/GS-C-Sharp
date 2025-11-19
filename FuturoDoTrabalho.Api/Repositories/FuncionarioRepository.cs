using Microsoft.EntityFrameworkCore;
using FuturoDoTrabalho.Api.Data;
using FuturoDoTrabalho.Api.Models;

namespace FuturoDoTrabalho.Api.Repositories
{
    public class FuncionarioRepository : GenericRepository<Funcionario>, IFuncionarioRepository
    {
        public FuncionarioRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Funcionario?> GetByCpfAsync(string cpf)
        {
            return await _context.Funcionarios
                .Include(f => f.Departamento)
                .FirstOrDefaultAsync(f => f.CPF == cpf);
        }

        public async Task<List<Funcionario>> GetByDepartamentoAsync(int departamentoId)
        {
            return await _context.Funcionarios
                .Where(f => f.DepartamentoId == departamentoId)
                .Include(f => f.Departamento)
                .ToListAsync();
        }

        public async Task<List<Funcionario>> GetByNivelSenioridadeAsync(int nivel)
        {
            return await _context.Funcionarios
                .Where(f => f.NivelSenioridade == nivel)
                .Include(f => f.Departamento)
                .ToListAsync();
        }

        public async Task<List<Funcionario>> GetAtivosAsync()
        {
            return await _context.Funcionarios
                .Where(f => f.Ativo)
                .Include(f => f.Departamento)
                .ToListAsync();
        }
    }
}
