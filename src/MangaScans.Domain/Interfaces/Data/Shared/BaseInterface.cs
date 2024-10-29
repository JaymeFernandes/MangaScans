using MangaScans.Domain.Entities;

namespace MangaScans.Data.Interfaces.Shared;

public interface IBaseRepository<TEntity> where TEntity : class
{
    public Task<List<TEntity>> GetAll();
    public Task<TEntity?> GetById(int id);
    public Task<TEntity?> GetById(string id);
    public Task<bool> AddAsync(TEntity entity);
    
    public Task<bool> UpdateAsync(TEntity entity);
    public Task<bool> DeleteAsync(TEntity entity);
    public Task<bool> DeleteByIdAsync(int id);
    public Task<bool> DeleteByIdAsync(string id);
}