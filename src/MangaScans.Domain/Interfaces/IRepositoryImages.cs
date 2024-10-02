using MangaScans.Data.Interfaces.Shared;
using MangaScans.Domain.Entities;

namespace MangaScans.Domain.Interfaces;

public interface IRepositoryImages : IBaseRepository<ImagesChapter>
{
    public Task<string> GetUrlById(int id);

    public Task<string> GenerateImageUrl(int chapterId);
}