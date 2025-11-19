using FuturoDoTrabalho.Api.Models;

namespace FuturoDoTrabalho.Api.Repositories
{
    public interface IDepartamentoRepository : IGenericRepository<Departamento>
    {
        Task<Departamento?> GetByNomeAsync(string nome);
        Task<List<Departamento>> GetAtivosAsync();
    }
}
