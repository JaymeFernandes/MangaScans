using MangaScans.Data.Context;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Repositories.Shared;

public class RepositoryChapter : BaseRepository<Chapter>, IRepositoryChapter
{

    public RepositoryChapter(AppDbContext context, UserManager<User> user) : base(context) { }

    public async override Task<bool> AddAsync(Chapter entity)
    {
        var manga = await _dbContext.Mangas.FirstOrDefaultAsync(x => x.Id == entity.IdManga);
        
        var result = await _dbContext.Chapters.
            Where(x => x.IdManga == entity.IdManga)
            .MaxAsync(x => x.Num);
        
        if(manga == null)
            return false;
        
        entity.Name = manga.Name;
        
        if (entity.Num == 0)
            entity.Num = result + 1;
        
        return await base.AddAsync(entity);
    }

    public async Task<List<Chapter>> GetChaptersByMangaId(string mangaId)
        => await _dbContext.Chapters
            .AsNoTracking()
            .Where(x => x.IdManga == mangaId)
            .OrderByDescending(x => x.Num)
            .ToListAsync();
    
    public override async Task<Chapter> GetById(int id)
        => await _dbContext.Chapters
            .AsNoTracking()
            .Include(c => c._Images)
            .FirstAsync(x => x.Id == id);

    public async Task<Chapter> GetByNum(string mangaId, int chapter, string? userId)
    {
        var entity = await _dbContext.Chapters
            .Include(x => x._Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Num == chapter && x.IdManga == mangaId);
        
        if (!string.IsNullOrEmpty(userId) && entity != null)
        {
            
        };
        
        

        return entity;
    }
}