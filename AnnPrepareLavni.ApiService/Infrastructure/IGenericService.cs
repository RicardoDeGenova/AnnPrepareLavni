namespace AnnPrepareLavni.ApiService.Infrastructure;

public interface IGenericService<T>
{
    public Task<T?> GetByIdAsync(Guid id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<bool> CreateAsync(T entity);
    public Task<bool> UpdateAsync(T entity);
    public Task<bool> DeleteAsync(Guid id);
}
