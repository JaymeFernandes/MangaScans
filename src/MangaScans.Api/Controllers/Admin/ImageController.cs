using MangaScans.Api.Controllers.Shared;
using MangaScans.Api.Services;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using MangaScans.Identity.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers;

[Route("api/admin/images")]
[Tags("images")]
public class ImageController : AdminBaseController
{
    private readonly IRepositoryImages _repositoryImages;

    public ImageController(IRepositoryImages repositoryImages, IHttpContextAccessor httpContextAccessor)
    {
        _repositoryImages = repositoryImages;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllImagesAsync()
        => Ok(await _repositoryImages.GetAll());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageByIdAsync(int id)
    {
        var entity = await _repositoryImages.GetById(id);

        if (entity == null)
            return NotFound();

        return Ok(entity);
    }
    
    [HttpPost("{idChapter}")]
    public async Task<IActionResult> UploadImageAsync([FromRoute] int idChapter, IFormFile file)
    {
        string path = await _repositoryImages.GenerateImageUrl(idChapter);
        ChaptersImagesService image = new ChaptersImagesService(path, idChapter);
        var imageEntity = await image.SaveImageAsync(file);

        if (imageEntity == null)
            return BadRequest();
        
        bool result = await _repositoryImages.AddAsync(imageEntity);

        if (!result)
            return StatusCode(500, "An error occurred while saving the image.");

        return Ok(new { result, Url = imageEntity.Url });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImageAsync(int id)
    {
        var result = await _repositoryImages.DeleteByIdAsync(id);

        if (!result)
            return StatusCode(500, "An error occurred while deleting the image.");
        
        return Ok();
    }
}
