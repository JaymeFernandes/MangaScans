using MangaScans.Api.Controllers.Shared;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers;

[Route("api/admin/images")]
[Tags("images")]
public class AdminImageController : CustomControllerBase
{
    private readonly IRepositoryImages _repositoryImages;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminImageController(IRepositoryImages repositoryImages, IHttpContextAccessor httpContextAccessor)
    {
        _repositoryImages = repositoryImages;
        _httpContextAccessor = httpContextAccessor;
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
        var request = _httpContextAccessor.HttpContext.Request;
        string baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        var extension = Path.GetExtension(file.FileName);
        
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");
        
        if (extension?.ToLower() != ".png" || file.ContentType != "image/png")
            return BadRequest("The file must be a PNG image.");

        string path = await _repositoryImages.GenerateImageUrl(idChapter);
        string basePath = $"{Directory.GetCurrentDirectory()}/StaticFiles/{Path.GetDirectoryName(path)}";
        string filePath = $"{basePath}/{Path.GetFileName(path)}";
        
        if (System.IO.File.Exists(filePath))
            return Conflict("File already exists.");

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);
        
        var imageEntity = new Images($"{baseUrl}/Images{path}", idChapter);
        bool result = await _repositoryImages.AddAsync(imageEntity);

        if (!result)
            return StatusCode(500, "An error occurred while saving the image.");

        return Ok(new { Url = imageEntity.Url });
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
