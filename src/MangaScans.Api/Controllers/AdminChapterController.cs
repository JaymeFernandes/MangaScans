using MangaScans.Application.DTOs.Request;
using MangaScans.Application.DTOs.Response;
using MangaScans.Data.Exceptions;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Shared;

[Tags("Private Routes (Admin)", "Chapters")]
[Route("api/chapter")]
public class AdminChapterController : CustomControllerBase
{
    protected readonly IRepositoryChapter _Chapter;
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var chapter = await _Chapter.GetAll();

        if (chapter.Count == 0) 
            throw new DbEntityException("there are no Chapter at the moment");

        return Ok(chapter.ToLibraryResponse());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var chapter = await _Chapter.GetById(id);

        if (chapter == null)
            return NotFound($"Chapter with id {id} not found");
        
        return Ok(chapter.ToLibraryResponse());
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] ChapterDtoRequest chapter)
    {
        if (chapter == null || string.IsNullOrEmpty(chapter.MangaId) || string.IsNullOrEmpty(chapter.Name))
            return BadRequest("Invalid chapter data.");

        var entity = new Chapter(chapter.MangaId, chapter.Name, chapter.numberChapter);

        bool result = await _Chapter.AddAsync(entity);

        if (!result)
            return BadRequest("Failed to add chapter.");

        return CreatedAtAction(nameof(GetAllAsync), new { id = entity.Id }, entity);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ChapterDtoRequest chapter)
    {
        var entity = await _Chapter.GetById(id);

        if (entity == null)
            return NotFound("Chapter with id {id} not found");

        bool result = await _Chapter.UpdateAsync(entity);

        if (!result)
            BadRequest();

        return Ok(entity.ToLibraryResponse());

    }
}