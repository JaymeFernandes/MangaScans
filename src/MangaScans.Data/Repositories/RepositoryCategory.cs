using MangaScans.Data.Context;
using MangaScans.Data.Repositories.Shared;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;

namespace MangaScans.Data.Repositories;

public class RepositoryCategory : BaseRepository<Category>, IRepositoryCategory
{
    public RepositoryCategory(AppDbContext dbContext) : base(dbContext) { }
}