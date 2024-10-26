using MangaScans.Data.Context;
using MangaScans.Data.Repositories.Shared;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Repositories;

public class RepositoryImages : BaseRepository<ImagesChapter>, IRepositoryImages
{
    public RepositoryImages(AppDbContext dbContext) : base(dbContext) { }

    public async override Task<bool> AddAsync(ImagesChapter entity)
    {
        var sequence = await _dbContext.Images
            .AsNoTracking()
            .Where(x => x.IdChapter == entity.IdChapter)
            .Select(x => (int?)x.Sequence)
            .MaxAsync() ?? 0;
        
        entity.Sequence = sequence + 1;
        
        return await base.AddAsync(entity);
    }

    public async Task<string> GetUrlById(int id)
        => (await _dbContext.Images.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync()).Url ?? string.Empty;

    public async Task<string> GenerateImageUrl(int chapterId)
    {
        Guid fileName = Guid.NewGuid();
        
        var chapter = await _dbContext.Chapters
            .AsNoTracking()
            .Include(x => x._Manga)
            .FirstOrDefaultAsync(c => c.Id == chapterId);

        if (chapter == null)
            return String.Empty;

        return $"{chapter._Manga.Id}/{chapter.Name}-{chapter.Num}/";
    }
}