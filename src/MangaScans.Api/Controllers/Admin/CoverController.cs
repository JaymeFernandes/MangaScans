using MangaScans.Api.Controllers.Shared;
using MangaScans.Domain.Interfaces;
using MangaScans.Api.Services;
using MangaScans.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Admin;

[Route("api/admin/cover")]
[Tags("cover")]
public class CoverController : BaseController
{
    protected readonly IRepositoryManga _repositoryManga;
    protected readonly IRepositoryCover _repositoryCover;

    public CoverController([FromServices] IRepositoryCover repositoryCover, [FromServices] IRepositoryManga repositoryManga)
    {
        _repositoryCover = repositoryCover;
        _repositoryManga = repositoryManga;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCovers()
     => Ok(await _repositoryCover.GetAll());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoverById([FromRoute] int id)
    {
        var entity = await _repositoryCover.GetById(id);
        if (entity == null)
            return NotFound();

        return Ok(entity);
    }

    [HttpPost("{idManga}")]
    public async Task<IActionResult> UploadImageCover([FromRoute] string idManga, IFormFile file)
    {
        var imageStream = new ImageService();
        
        var manga = await _repositoryManga.GetById(idManga);
        
        if (manga == null)
            return NotFound();
        
        var cover = await imageStream.SaveCoverImageAsync(file, manga.Name, manga.Id);

        if (cover == null)
            return BadRequest();

        var result = await _repositoryCover.AddAsync(cover);
        
        return Ok();
    }
}