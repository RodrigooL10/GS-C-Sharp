using FuturoDoTrabalho.Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuturoDoTrabalho.Api.Services
{
    public interface ITrabalhadorService
    {
        Task<TrabalhadorReadDto> GetByIdAsync(int id);
        Task<IEnumerable<TrabalhadorReadDto>> GetAllAsync(bool? ativo = null);
        Task<IEnumerable<TrabalhadorReadDto>> GetPagedAsync(int pageNumber, int pageSize, bool? ativo = null);
        Task<int> GetCountAsync(bool? ativo = null);
        Task<TrabalhadorReadDto> CreateAsync(TrabalhadorCreateDto dto);
        Task<TrabalhadorReadDto> UpdateAsync(int id, TrabalhadorUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
