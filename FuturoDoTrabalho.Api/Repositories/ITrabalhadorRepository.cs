using FuturoDoTrabalho.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuturoDoTrabalho.Api.Repositories
{
    public interface ITrabalhadorRepository
    {
        Task<Trabalhador> GetByIdAsync(int id);
        Task<IEnumerable<Trabalhador>> GetAllAsync(bool? ativo = null);
        Task<Trabalhador> GetByCpfAsync(string cpf);
        Task<Trabalhador> CreateAsync(Trabalhador trabalhador);
        Task<Trabalhador> UpdateAsync(Trabalhador trabalhador);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync(bool? ativo = null);
        Task<IEnumerable<Trabalhador>> GetPagedAsync(int pageNumber, int pageSize, bool? ativo = null);
    }
}
