using MangaScans.Data.Interfaces.Shared;
using MangaScans.Domain.Entities;

namespace MangaScans.Domain.Interfaces;

public interface IRepositoryManga : IBaseRepository<Manga>
{
    public Task<List<Manga>> GetTop(int page);

    public Task<List<Manga>> GetTopByCategory(int page, int Category);

    public Task<List<Manga>> SearchByName(string name, int page);

    public Task<bool> UpdateAsync(Manga mangaDto, string Id);
}