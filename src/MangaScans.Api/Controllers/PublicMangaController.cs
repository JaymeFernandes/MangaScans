using MangaScans.Api.Controllers.Shared;
using MangaScans.Application.DTOs.Response;
using MangaScans.Data.Exceptions;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers;

/// <summary>
/// PublicController handles public-facing routes for retrieving manga recommendations, 
/// searching by title, and getting specific manga details.
/// </summary>
[Tags("Public Routes")]
public class PublicMangaController : CustomControllerBase
{
    protected readonly IRepositoryManga _repositoryManga;

    /// <summary>
    /// Initializes a new instance of the PublicController class.
    /// </summary>
    /// <param name="manga">The manga repository service injected via dependency injection.</param>
    public PublicMangaController([FromServices] IRepositoryManga manga) => _repositoryManga = manga;

    /// <summary>
    /// Redirects to the first page of top manga recommendations.
    /// </summary>
    /// <returns>A redirect to the default page (1).</returns>
    [HttpGet("recommendation")]
    public IActionResult TopMangas() => Redirect("1");

    /// <summary>
    /// Retrieves the top manga recommendations for a specified page.
    /// </summary>
    /// <param name="page">The page number of recommendations to retrieve.</param>
    /// <returns>A list of recommended mangas on the specified page.</returns>
    [HttpGet("recommendation/{page}")]
    public async Task<IActionResult> GetTopMangas([FromRoute] int page)
    {
        if (page == null || page == 0) 
            return Redirect($"/api/recommendation/1");

        var mangas = await _repositoryManga.GetTop(page);

        if (mangas.Count == 0) 
            return NotFound();

        return Ok(mangas.TolibraryResponse());
    }

    /// <summary>
    /// Retrieves the top manga recommendations for a specific category and page.
    /// </summary>
    /// <param name="page">The page number of recommendations to retrieve.</param>
    /// <param name="category">The category ID to filter the recommendations by.</param>
    /// <returns>A list of recommended mangas in the specified category on the specified page.</returns>
    [HttpGet("recommendation/{page}/{category}")]
    public async Task<IActionResult> GetTopMangas([FromRoute] int page, [FromRoute] int category)
    {
        var mangas = await _repositoryManga.GetTopByCategory(page, category);

        if (mangas.Count == 0) 
            return NotFound();

        return Ok(mangas.TolibraryResponse());
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

        return Ok(manga.ToLibraryResponse());
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

        return Ok(mangas.TolibraryResponse());
    }
}
