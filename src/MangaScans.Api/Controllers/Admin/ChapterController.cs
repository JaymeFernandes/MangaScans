using MangaScans.Application.DTOs.Request;
using MangaScans.Application.DTOs.Response;
using MangaScans.Application.DTOs.Response.Public_Routes;
using MangaScans.Data.Exceptions;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using MangaScans.Identity.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Shared;

/// <summary>
/// ChapterController provides endpoints for managing chapters, including CRUD operations.
/// </summary>
[Tags("Chapters")]
[Route("api/admin/chapter")]
public class ChapterController : AdminBaseController
{
    protected readonly IRepositoryChapter _chapterRepository;
    
    /// <summary>
    /// Initializes a new instance of the ChapterController class.
    /// </summary>
    /// <param name="chapterRepository">The chapter repository service injected via dependency injection.</param>
    public ChapterController([FromServices] IRepositoryChapter chapterRepository) => _chapterRepository = chapterRepository;
    
    /// <summary>
    /// Retrieves all chapters.
    /// </summary>
    /// <returns>A list of all chapters.</returns>
    /// <exception cref="DbEntityException">Thrown when no chapters are found.</exception>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var chapters = await _chapterRepository.GetAll();

        if (chapters.Count == 0) 
            throw new DbEntityException("No chapters are available at the moment.");

        return Ok(chapters.ToLibraryResponse());
    }

    /// <summary>
    /// Retrieves a chapter by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the chapter.</param>
    /// <returns>The chapter with the specified ID, if found.</returns>
    /// <response code="404">Chapter with the specified ID was not found.</response>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var chapter = await _chapterRepository.GetById(id);

        if (chapter == null)
            return NotFound($"Chapter with ID {id} not found.");
        
        return Ok(chapter.ToLibraryResponse());
    }

    /// <summary>
    /// Adds a new chapter.
    /// </summary>
    /// <param name="chapter">The chapter data to add.</param>
    /// <returns>The created chapter, or an error if creation fails.</returns>
    /// <response code="400">Invalid chapter data or failed to add chapter.</response>
    [HttpPost]
    public async Task<IActionResult> CreateChapter([FromBody] ChapterDtoRequest chapter)
    {
        if (chapter == null || string.IsNullOrEmpty(chapter.MangaId) || string.IsNullOrEmpty(chapter.Name))
            return BadRequest("Invalid chapter data. Please provide valid MangaId and Name.");

        var entity = new Chapter(chapter.MangaId, chapter.Name, chapter.NumberChapter);

        bool result = await _chapterRepository.AddAsync(entity);

        if (!result)
            return BadRequest("Failed to add chapter. Please try again later.");

        return Ok();
    }

    /// <summary>
    /// Updates an existing chapter by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the chapter to update.</param>
    /// <param name="chapter">The updated chapter data.</param>
    /// <returns>The updated chapter, or an error if the update fails.</returns>
    /// <response code="404">Chapter with the specified ID was not found.</response>
    /// <response code="400">Failed to update chapter.</response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ChapterDtoRequest chapter)
    {
        var existingChapter = await _chapterRepository.GetById(id);

        if (existingChapter == null)
            return NotFound($"Chapter with ID {id} not found.");

        // Update the existing chapter with new data
        existingChapter.Name = chapter.Name;
        existingChapter.Num = chapter.NumberChapter;

        bool result = await _chapterRepository.UpdateAsync(existingChapter);

        if (!result)
            return BadRequest("Failed to update chapter. Please try again later.");

        return Ok(existingChapter.ToLibraryResponse());
    }

    /// <summary>
    /// Deletes a chapter by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the chapter to delete.</param>
    /// <returns>A confirmation of deletion or an error if deletion fails.</returns>
    /// <response code="404">Chapter with the specified ID was not found.</response>
    /// <response code="400">Failed to delete chapter.</response>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var chapter = await _chapterRepository.GetById(id);
        var result = await _chapterRepository.DeleteByIdAsync(id); 

        if (chapter == null)
            return NotFound($"Chapter with ID {id} not found.");

        if (!result)
            throw new DbEntityException("An error occurred while attempting to delete the chapter.");

        return Ok("Chapter deleted successfully.");
    }
}
