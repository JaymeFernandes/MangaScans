using MangaScans.Api.Controllers.Shared;
using MangaScans.Application.DTOs.Response;
using MangaScans.Data.Exceptions;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers;

[Tags("Public Routes", "Mangas")]
public class MangaController : CustomControllerBase
{
    protected readonly IRepositoryManga _repositoryManga;
    
    public MangaController([FromServices] IRepositoryManga manga) => _repositoryManga = manga;
    
    [HttpGet("recommendation")]
    public IActionResult TopMangas() => Redirect("1");

    [HttpGet("recommendation/{page}")]
    public async Task<IActionResult> GetTopMangas([FromRoute] int page)
    {
        if (page == null || page == 0) Redirect($"/api/recommendation/1");
        
        var mangas = await _repositoryManga.GetTop(page);

        if (mangas.Count == 0) NotFound();
        
        return Ok(mangas.TolibraryResponse());
    }

    [HttpGet("recommendation/{page}/{category}")]
    public async Task<IActionResult> GetTopMangas([FromRoute] int page, [FromRoute] int category)
    {
        var mangas = await _repositoryManga.GetTopByCategory(page, category);
        
        if (mangas.Count == 0) NotFound();
        
        return Ok(mangas.TolibraryResponse());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var manga = await _repositoryManga.GetById(id) ??
                    throw new DbEntityException($"There is no manga with id {id}");

        return Ok(manga.ToLibraryResponse());
    }

    [HttpGet("search/{title}/{page}")]
    public async Task<IActionResult> Search([FromRoute] string title, int page)
    {
        var mangas = await _repositoryManga.SearchByName(title, page);

        if (mangas.Count == 0) return NotFound();

        return Ok(mangas.TolibraryResponse());
    }

}