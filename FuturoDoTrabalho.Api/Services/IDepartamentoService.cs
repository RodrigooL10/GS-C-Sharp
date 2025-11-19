using FuturoDoTrabalho.Api.DTOs;

namespace FuturoDoTrabalho.Api.Services
{
    public interface IDepartamentoService
    {
        Task<DepartamentoReadDto?> GetByIdAsync(int id);
        Task<IEnumerable<DepartamentoReadDto>> GetAllAsync();
        Task<(IEnumerable<DepartamentoReadDto> data, int totalCount, int pageCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<DepartamentoReadDto?> GetByNomeAsync(string nome);
        Task<IEnumerable<DepartamentoReadDto>> GetAtivosAsync();
        Task<DepartamentoReadDto> CreateAsync(DepartamentoCreateDto dto);
        Task<DepartamentoReadDto?> UpdateAsync(int id, DepartamentoUpdateDto dto);
        Task<DepartamentoReadDto?> PatchAsync(int id, DepartamentoPatchDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
