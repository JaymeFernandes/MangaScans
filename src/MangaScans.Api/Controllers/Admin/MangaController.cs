using MangaScans.Api.Controllers.Shared;
using MangaScans.Application.DTOs.Request;
using MangaScans.Data.Exceptions;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers;

/// <summary>
/// MangaController provides endpoints for managing mangas, including CRUD operations and category management.
/// </summary>
[Tags("Mangas")]
[Route("api/admin/manga")]
public class MangaController : BaseController
{
    private readonly IRepositoryManga _mangaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the MangaController class.
    /// </summary>
    /// <param name="mangaRepository">The manga repository service injected via dependency injection.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor injected via dependency injection.</param>
    public MangaController([FromServices] IRepositoryManga mangaRepository, [FromServices] IHttpContextAccessor httpContextAccessor)
    {
        _mangaRepository = mangaRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Retrieves all mangas from the database.
    /// </summary>
    /// <returns>A list of all mangas.</returns>
    /// <response code="200">Returns the list of mangas.</response>
    /// <response code="404">Throws an exception if no mangas are found.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MangaDtoRequest>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> GetAllAsync()
    {
        var mangas = await _mangaRepository.GetAll();

        if (mangas.Count == 0)
            throw new DbEntityException("No mangas available at the moment.");

        return Ok(mangas);
    }

    /// <summary>
    /// Adds a new manga to the database.
    /// </summary>
    /// <param name="manga">DTO object containing the data for the manga to be added.</param>
    /// <returns>Returns the newly added manga.</returns>
    /// <response code="201">Manga successfully added.</response>
    /// <response code="400">Throws an exception if there was an error adding the manga.</response>
    [HttpPost]
    [ProducesResponseType(typeof(MangaDtoRequest), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> AddAsync([FromBody] MangaDtoRequest manga)
    {
        if (manga == null || string.IsNullOrEmpty(manga.Name) || string.IsNullOrEmpty(manga.Description))
            return BadRequest("Invalid manga data provided.");

        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

        var entity = new Manga(manga.Name, manga.Description);
        var response = await _mangaRepository.AddAsync(entity);

        if (!response)
            throw new DbEntityException("Error occurred while adding the manga.");

        return Created($"{baseUrl}/api/manga/{entity.Id}", entity);
    }

    /// <summary>
    /// Updates an existing manga.
    /// </summary>
    /// <param name="id">The unique identifier of the manga to update.</param>
    /// <param name="manga">DTO object containing the updated data for the manga.</param>
    /// <returns>Returns a status indicating whether the update was successful.</returns>
    /// <response code="200">Manga successfully updated.</response>
    /// <response code="400">Throws an exception if the update fails.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] MangaDtoRequest manga)
    {
        if (string.IsNullOrEmpty(id) || manga == null)
            return BadRequest("Invalid manga data or ID.");

        var result = await _mangaRepository.UpdateAsync(new Manga(manga.Name, manga.Description), id);

        if (!result)
            throw new DbEntityException("Error occurred while updating the manga.");

        return Ok();
    }

    /// <summary>
    /// Adds a category to a manga.
    /// </summary>
    /// <param name="category">DTO object containing the manga ID and category ID to be added.</param>
    /// <returns>Returns a status indicating whether the category was successfully added.</returns>
    /// <response code="200">Category successfully added to the manga.</response>
    /// <response code="400">Throws an exception if there was an error adding the category.</response>
    [HttpPost("Category")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryMangaDtoRequest category)
    {
        if (category == null || string.IsNullOrEmpty(category.id_manga) || category.id_category == 0)
            return BadRequest("Invalid category data provided.");

        var result = await _mangaRepository.AddCategory(category.id_manga, category.id_category);

        if (!result)
            throw new DbEntityException("Error occurred while adding the category to the manga.");

        return Ok();
    }

    /// <summary>
    /// Deletes a manga by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the manga to be deleted.</param>
    /// <returns>Returns a status indicating whether the deletion was successful.</returns>
    /// <response code="200">The manga was successfully deleted.</response>
    /// <response code="400">Throws a BadRequest if the provided ID is invalid or if the deletion fails.</response>
    /// [ProducesResponseType(typeof(void), 200)]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(typeof(Chapter), 400)]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        bool result = await _mangaRepository.DeleteByIdAsync(id);

        if (!result)
            return BadRequest("Invalid ID.");
    
        return Ok();
    }
}
