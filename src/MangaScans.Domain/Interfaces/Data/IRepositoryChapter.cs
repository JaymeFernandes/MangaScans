using MangaScans.Data.Interfaces.Shared;
using MangaScans.Domain.Entities;

namespace MangaScans.Domain.Interfaces.Data;

public interface IRepositoryChapter : IBaseRepository<Chapter>
{
    public Task<Chapter?> GetByNum(string mangaId, int chapter, string? userId);
}