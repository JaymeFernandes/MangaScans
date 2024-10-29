using MangaScans.Api.Controllers.Shared;
using MangaScans.Domain.Interfaces;
using MangaScans.Api.Services;
using MangaScans.Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Admin;

/// <summary>
/// CoverController handles administrative operations related to manga cover images.
/// </summary>
[Route("api/admin/cover")]
[Tags("cover")]
public class CoverController : AdminBaseController
{
    private readonly IRepositoryManga _repositoryManga;
    private readonly IRepositoryCover _repositoryCover;

    /// <summary>
    /// Initializes a new instance of the CoverController class with injected cover and manga repositories.
    /// </summary>
    /// <param name="repositoryCover">Service for managing cover-related data operations.</param>
    /// <param name="repositoryManga">Service for managing manga-related data operations.</param>
    public CoverController([FromServices] IRepositoryCover repositoryCover, [FromServices] IRepositoryManga repositoryManga)
    {
        _repositoryCover = repositoryCover;
        _repositoryManga = repositoryManga;
    }

    /// <summary>
    /// Retrieves all cover images from the database.
    /// </summary>
    /// <returns>A list of all available cover images.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCovers()
     => Ok(await _repositoryCover.GetAll());

    /// <summary>
    /// Retrieves a specific cover image by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the cover image.</param>
    /// <returns>The cover image with the specified ID, or a 404 status if not found.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoverById([FromRoute] int id)
    {
        var entity = await _repositoryCover.GetById(id);
        
        if (entity == null)
            return NotFound();

        return Ok(entity);
    }

    /// <summary>
    /// Uploads a new cover image for a specified manga.
    /// </summary>
    /// <param name="idManga">The unique identifier of the manga for which the cover is being uploaded.</param>
    /// <param name="file">The image file for the cover.</param>
    /// <returns>Status 200 OK if successful, or an error status if the process fails.</returns>
    [HttpPost("{idManga}")]
    public async Task<IActionResult> UploadImageCover([FromRoute] string idManga, IFormFile file)
    {
        var imageStream = new ImageCoverService();
        
        var manga = await _repositoryManga.GetById(idManga);
        
        if (manga == null)
            return NotFound();
        
        var cover = await imageStream.SaveCoverImageAsync(file, manga.Name, manga.Id);

        if (cover == null)
            return BadRequest();

        await _repositoryCover.AddAsync(cover);
        
        return Ok();
    }
}
