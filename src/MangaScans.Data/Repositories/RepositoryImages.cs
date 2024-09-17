using MangaScans.Data.Context;
using MangaScans.Data.Repositories.Shared;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;

namespace MangaScans.Data.Repositories;

public class RepositoryImages : BaseRepository<Images>, IRepositoryImages
{
    public RepositoryImages(AppDbContext dbContext) : base(dbContext) { }
}