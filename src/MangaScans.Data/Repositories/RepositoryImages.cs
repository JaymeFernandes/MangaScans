using MangaScans.Data.Context;
using MangaScans.Data.Repositories.Shared;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Repositories;

public class RepositoryImages : BaseRepository<Images>, IRepositoryImages
{
    public RepositoryImages(AppDbContext dbContext) : base(dbContext) { }

    public async Task<string> GetUrlById(int id)
        => (await _dbContext.Images.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync()).Url ?? string.Empty;
}