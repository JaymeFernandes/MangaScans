using System.Security.Claims;
using MangaScans.Api.Controllers.Shared;
using MangaScans.Application.DTOs.Response;
using MangaScans.Application.DTOs.Response.Public_Routes;
using MangaScans.Data.Exceptions;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using MangaScans.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers;

/// <summary>
/// PublicController handles public-facing routes for retrieving manga recommendations, 
/// searching by title, and getting specific manga details.
/// </summary>
[Tags("Public Routes")]
public class PublicController : BaseController
{
    protected readonly IRepositoryManga _repositoryManga;
    protected readonly IRepositoryChapter _repositoryChapter;
    protected readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the PublicController class.
    /// </summary>
    /// <param name="manga">The manga repository service injected via dependency injection.</param>
    public PublicController([FromServices] IRepositoryManga manga, [FromServices] IUserRepository userRepository, [FromServices] IRepositoryChapter repositoryChapter)
    {
        _repositoryManga = manga;
        _repositoryChapter = repositoryChapter;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Retrieves the top manga recommendations for a specified page.
    /// </summary>
    /// <param name="page">The page number of recommendations to retrieve.</param>
    /// <returns>A list of recommended mangas on the specified page.</returns>
    [HttpGet("recommendation")]
    public async Task<IActionResult> GetTopMangas([FromHeader(Name = "Page")] int page, 
                                                  [FromHeader(Name = "Categories")] List<int> categories)
    {
        List<Manga> mangas;
        int pages = 0;

        if (page == 0)
            return BadRequest();

        if (categories.Count() == 0 || categories == null)
        {
            mangas = await _repositoryManga.GetTop(page);
            if (mangas.Count > 0)
                pages = await _repositoryManga.GetTopCount();
        }
        else
        {
            mangas = await _repositoryManga.GetTopByCategories(page, categories);
            if (mangas.Count > 0)
                pages = await _repositoryManga.GetTopByCategoriesPageCount(categories);
        }

        if (mangas.Count == 0) 
            return NotFound();

        return Ok(new { Pages = pages, Data = mangas.RecommendationToLibraryResponse() });
    }
    
    /// <summary>
    /// Searches for mangas by title and retrieves the results for a specified page.
    /// </summary>
    /// <param name="title">The title or part of the title to search for.</param>
    /// <param name="page">The page number of search results to retrieve.</param>
    /// <returns>A list of mangas that match the search criteria on the specified page.</returns>
    [HttpGet("search/{page}/{title}")]
    public async Task<IActionResult> Search([FromRoute] string title, [FromRoute] int page)
    {
        var mangas = await _repositoryManga.SearchByName(title, page);

        if (mangas.Count == 0) 
            return NotFound();

        return Ok(new {Data = mangas.RecommendationToLibraryResponse()});
    }

    /// <summary>
    /// Retrieves a manga by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the manga.</param>
    /// <returns>The manga with the specified ID.</returns>
    /// <exception cref="DbEntityException">Thrown when no manga is found with the specified ID.</exception>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var manga = await _repositoryManga.GetById(id) ??
                    throw new DbEntityException($"There is no manga with id {id}");

        return Ok(new {Data = manga.ToLibraryResponse() });
    }

    [HttpGet("{mangaId}/{num:int}")]
    public async Task<IActionResult> GetChapterByNum([FromRoute] string mangaId, [FromRoute] int num)
    {
        var userId = "";
        
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            
            userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        
        var chapter = await _repositoryChapter.GetByNum(mangaId, num, userId);

        if (chapter != null)
            return Ok(chapter.ToLibraryResponse());

        return BadRequest();
    }
    
}
