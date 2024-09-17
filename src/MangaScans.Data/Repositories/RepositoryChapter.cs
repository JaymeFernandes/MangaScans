using MangaScans.Data.Context;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;

namespace MangaScans.Data.Repositories.Shared;

public class RepositoryChapter : BaseRepository<Chapter>, IRepositoryChapter
{
    public RepositoryChapter(AppDbContext context) : base(context) { }
}