using MangaScans.Application.DTOs.Request;
using MangaScans.Application.DTOs.Response;
using MangaScans.Data.Context;
using MangaScans.Data.Repositories.Shared;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Repositories;

public class RepositoryManga : BaseRepository<Manga>, IRepositoryManga
{
    public RepositoryManga(AppDbContext dbContext) : base(dbContext) { }

    public override async Task<Manga> GetById(string id)
    {
        var manga = await _dbContext.Mangas
            .Include(x => x.Categories)
            .Include(x => x.Chapters)
            .FirstAsync(x => x.Id == id);
        
        if (manga != null)
        {
            manga.Views += 1;
            await _dbContext.SaveChangesAsync();
        }

        return manga;
    }

    public async Task<List<Manga>> GetTop(int page)
        => await _dbContext.Mangas
            .Include(c => c.Categories)
            .Include(m => m.Cover)
            .AsNoTracking()
            .OrderByDescending(m => m.Likes  * 0.7 + m.Views * 0.3)
            .Skip((page - 1) * 10)
            .Take(10)
            .ToListAsync();

    public async Task<List<Manga>> GetTopByCategory(int page, int Category)
        => await _dbContext.Mangas
            .Include(c => c.Categories)
            .AsNoTracking()
            .Where(x => x.Categories.Any(x => x.Id == Category))
            .OrderByDescending(m => m.Likes * 0.7 + m.Views * 0.3)
            .Skip((page - 1) * 10)
            .Take(10)
            .ToListAsync();

    public async Task<List<Manga>> SearchByName(string name, int page)
        => await _dbContext.Mangas
            .Include(c => c.Categories)
            .AsNoTracking()
            .Where(m => m.Name.Contains(name) || m.Description.Contains(name))
            .OrderByDescending(m => m.Likes * 0.7 + m.Views * 0.3)
            .Skip((page - 1) * 10)
            .Take(10)
            .ToListAsync();

    public async Task<bool> UpdateAsync(Manga mangaDto, string Id)
    {
        var manga = await GetById(Id);
        
        manga.Name = mangaDto.Name;
        manga.Description = mangaDto.Description;

        return await UpdateAsync(manga);
    }

    public async Task<bool> AddCategory(string id, int categoryId)
    {
        var entity = await GetById(id);

        if (entity == null)
            return false;
        
        CategoryManga category = new CategoryManga()
        {
            CategoryId = categoryId,
            MangaId = id
        };

        await _dbContext.AddAsync(category);
        
        var result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }
}