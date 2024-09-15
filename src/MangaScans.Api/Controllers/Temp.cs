using MangaScans.Data.Context;
using MangaScans.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControllerBase = MangaScans.Api.Controllers.Shared.ControllerBase;

namespace MangaScans.Api.Controllers;

public class Temp : ControllerBase
{
    protected readonly AppDbContext _context;
    
    public Temp([FromServices] AppDbContext context) => _context = context;

    [HttpPost("AddManga")]
    public async Task<IActionResult> AddManga()
    {
        Manga manga = new Manga(1, "Test1", "Temstkds");
        await _context.Mangas.AddAsync(manga);
        await _context.SaveChangesAsync();
        
        return Ok(manga);
    }
    
    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory([FromQuery] string mangaId)
    {

        // Adiciona capítulos ao Manga
        var chapter1 = new Chapter(mangaId,"Chapter 1: The Beginnin", 2)
        {
            Name = "Chapter 1: The Beginning",
            Num = 1,
            CreatedAt = DateTime.Now
        };

        var chapter2 = new Chapter(mangaId,"Chapter 1: The Beginnin", 3)
        {
            Name = "Chapter 2: The Journey Continues",
            Num = 2,
            CreatedAt = DateTime.Now
        };

        _context.Chapters.Add(chapter1);
        _context.Chapters.Add(chapter2);
        
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{mangaId}")]
    public async Task<IActionResult> GetManga(string mangaId)
    {
        var manga = await _context.Mangas
            .Include(x => x._Chapters)
            .FirstOrDefaultAsync(m => m.Id == mangaId);
        
        if (manga == null)
        {
            return NotFound();
        }

        MangaDto mangaDto = new()
        {
            Id = manga.Id,
            Name = manga.Name,
            Description = manga.Description,
            CreatedAt = manga.CreatedAt,
            ChaptersSave = manga._Chapters
        };
        
        return Ok(mangaDto);
    }
}