using FuturoDoTrabalho.Api.Models;

namespace FuturoDoTrabalho.Api.Repositories
{
    public interface IFuncionarioRepository : IGenericRepository<Funcionario>
    {
        Task<Funcionario?> GetByCpfAsync(string cpf);
        Task<List<Funcionario>> GetByDepartamentoAsync(int departamentoId);
        Task<List<Funcionario>> GetByNivelSenioridadeAsync(int nivel);
        Task<List<Funcionario>> GetAtivosAsync();
    }
}
