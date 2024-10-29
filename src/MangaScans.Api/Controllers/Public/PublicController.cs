using System.Security.Claims;
using MangaScans.Api.Controllers.Shared;
using MangaScans.Application.DTOs.Response.Public_Routes;
using MangaScans.Data.Exceptions;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Public;

/// <summary>
/// PublicController handles public-facing routes for retrieving manga recommendations, 
/// searching by title, and accessing specific manga details.
/// </summary>
[Tags("Public Routes")]
public class PublicController : BaseController
{
    private readonly IRepositoryManga _repositoryManga;
    private readonly IRepositoryChapter _repositoryChapter;

    /// <summary>
    /// Initializes a new instance of the PublicController class with dependency injection.
    /// </summary>
    /// <param name="manga">Injected manga repository service.</param>
    /// <param name="repositoryChapter">Injected chapter repository service.</param>
    public PublicController([FromServices] IRepositoryManga manga, [FromServices] IRepositoryChapter repositoryChapter)
    {
        _repositoryManga = manga;
        _repositoryChapter = repositoryChapter;
    }

    /// <summary>
    /// Retrieves top manga recommendations for a specified page.
    /// </summary>
    /// <param name="page">Page number of recommendations to retrieve.</param>
    /// <param name="categories">Optional category filter for recommendations.</param>
    /// <returns>A paginated list of recommended mangas.</returns>
    [HttpGet("recommendation")]
    public async Task<IActionResult> GetTopMangas([FromHeader(Name = "Page")] int page, [FromHeader(Name = "Categories")] List<int> categories)
    {
        List<Manga>? mangas;
        int pages = 0;

        if (page <= 0)
            return BadRequest();

        if (categories.Count == 0)
        {
            mangas = await _repositoryManga.GetTop(page);
            
            if (mangas == null)
                return NotFound();
            
            if (mangas.Count > 0)
                pages = await _repositoryManga.GetTopCount();
        }
        else
        {
            mangas = await _repositoryManga.GetTopByCategories(page, categories);

            if (mangas == null)
             return NotFound();
            
            if (mangas.Count > 0)
                pages = await _repositoryManga.GetTopByCategoriesPageCount(categories);
        }

        if (mangas.Count == 0) 
            return NotFound();
        
        return Ok(new { Pages = pages, Data = mangas.RecommendationToLibraryResponse() });
    }

    /// <summary>
    /// Searches mangas by title and retrieves results for the specified page.
    /// </summary>
    /// <param name="title">The title or part of the title to search for.</param>
    /// <param name="page">Page number of search results to retrieve.</param>
    /// <returns>A list of mangas that match the search criteria.</returns>
    [HttpGet("search/{page}/{title}")]
    public async Task<IActionResult> Search([FromRoute] string title, [FromRoute] int page)
    {
        var mangas = await _repositoryManga.SearchByName(title, page);
        
        if (mangas == null) 
            return NotFound();
        
        if (mangas.Count == 0) 
            return NotFound();

        return Ok(new { Data = mangas.RecommendationToLibraryResponse() });
    }

    /// <summary>
    /// Retrieves manga details by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the manga.</param>
    /// <returns>The manga with the specified ID.</returns>
    /// <exception cref="DbEntityException">Thrown when no manga is found with the specified ID.</exception>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var manga = await _repositoryManga.GetById(id) ??
                    throw new DbEntityException($"There is no manga with id {id}");

        return Ok(new { Data = manga.ToLibraryResponse() });
    }

    /// <summary>
    /// Retrieves a specific chapter of a manga by the chapter number and manga ID.
    /// </summary>
    /// <param name="mangaId">The unique identifier of the manga.</param>
    /// <param name="num">The number of the chapter to retrieve.</param>
    /// <returns>The data of the specified chapter.</returns>
    [HttpGet("{mangaId}/{num:int}")]
    public async Task<IActionResult> GetChapterByNum([FromRoute] string mangaId, [FromRoute] int num)
    {
        var user = HttpContext.User.Identity as ClaimsIdentity;
        string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        var chapter = await _repositoryChapter.GetByNum(mangaId, num, userId);

        if (chapter != null)
            return Ok(chapter.ToLibraryResponse());

        return BadRequest();
    }
}
