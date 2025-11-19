using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Models;

namespace FuturoDoTrabalho.Api.Services
{
    public interface IFuncionarioService
    {
        Task<FuncionarioReadDto?> GetByIdAsync(int id);
        Task<List<FuncionarioReadDto>> GetAllAsync();
        Task<(List<FuncionarioReadDto> data, int totalCount, int pageCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<FuncionarioReadDto?> GetByCpfAsync(string cpf);
        Task<List<FuncionarioReadDto>> GetByDepartamentoAsync(int departamentoId);
        Task<List<FuncionarioReadDto>> GetAtivosAsync();
        Task<FuncionarioReadDto> CreateAsync(FuncionarioCreateDto dto);
        Task<FuncionarioReadDto?> UpdateAsync(int id, FuncionarioUpdateDto dto);
        Task<FuncionarioReadDto?> PatchAsync(int id, FuncionarioPatchDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
