using MangaScans.Data.Context;
using MangaScans.Data.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Repositories.Shared;

public abstract class BaseRepository<TEntity> : IDisposable, IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _dbContext;
    
    public BaseRepository(AppDbContext dbContext) => _dbContext = dbContext;

    //Get
    public virtual async Task<List<TEntity>> GetAll() 
            => await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();


    public virtual async Task<TEntity> GetById(int id)
        => await _dbContext.Set<TEntity>()
            .FindAsync(id);
    
    public virtual async Task<TEntity> GetById(string id)
        => await _dbContext.Set<TEntity>()
            .FindAsync(id);
    
    //Add
    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
        int result = await _dbContext.SaveChangesAsync();
        
        return result > 0;
    }
    
    //Update
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        int result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }
    
    
    //Delete
    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }
    
    public virtual async Task<bool> DeleteByIdAsync(int id)
    {
        
        var entity = await GetById(id);

        if (entity == null) return false;

        return await DeleteAsync(entity);
    }

    public virtual async Task<bool> DeleteByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return false;
        
        var entity = await GetById(id);
        
        if (entity == null) 
            return false;

        return await DeleteAsync(entity);
    }
    
    
    public void Dispose()
    {
        _dbContext.Dispose();
    }
}