using MangaScans.Data.Interfaces.Shared;
using MangaScans.Domain.Entities;

namespace MangaScans.Domain.Interfaces;

public interface IRepositoryImages : IBaseRepository<Images>
{
    public Task<string> GetUrlById(int id);
}