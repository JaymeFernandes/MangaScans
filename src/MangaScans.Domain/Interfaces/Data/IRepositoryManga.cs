using MangaScans.Data.Interfaces.Shared;
using MangaScans.Domain.Entities;

namespace MangaScans.Domain.Interfaces;

public interface IRepositoryManga : IBaseRepository<Manga>
{
    public Task<List<Manga>> GetTop(int page);

    public Task<int> GetTopCount();

    public Task<List<Manga>> GetTopByCategories(int page, List<int> categories);

    public Task<int> GetTopByCategoriesPageCount(List<int> categories);

    public Task<List<Manga>> SearchByName(string name, int page);

    public Task<bool> UpdateAsync(Manga mangaDto, string Id);

    public Task<bool> AddCategory(string id, int categoryId);
}