using MangaScans.Data.Context;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Repositories.Shared;

public class RepositoryChapter : BaseRepository<Chapter>, IRepositoryChapter
{
    public RepositoryChapter(AppDbContext context) : base(context) { }

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
}