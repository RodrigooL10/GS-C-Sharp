namespace FuturoDoTrabalho.Api.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> GetCountAsync();
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
