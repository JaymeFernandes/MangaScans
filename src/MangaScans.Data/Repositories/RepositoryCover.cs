using MangaScans.Data.Context;
using MangaScans.Data.Repositories.Shared;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;

namespace MangaScans.Data.Repositories;

public class RepositoryCover : BaseRepository<ImagesCover>, IRepositoryCover
{
    public RepositoryCover(AppDbContext dbContext) : base(dbContext) { }
}