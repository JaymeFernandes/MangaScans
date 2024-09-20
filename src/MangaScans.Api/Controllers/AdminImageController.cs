using MangaScans.Api.Controllers.Shared;
using MangaScans.Data.Repositories;
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

    [HttpPut("{idChapter}")]
    public async Task<IActionResult> UploadImage([FromRoute] int idChapter, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");
        
        var extension = Path.GetExtension(file.FileName);
        if (extension?.ToLower() != ".png")
            return BadRequest("The file must be a PNG image.");

        if (file.ContentType != "image/png")
            return BadRequest("The file must have a content type of image/png.");
        
        string id = Guid.NewGuid().ToString();
        var request = _httpContextAccessor.HttpContext.Request;
        string baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
        string filePath = Path.Combine(directoryPath, $"{id}.png");

        if (System.IO.File.Exists(filePath))
            return Conflict("File already exists.");

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var imageEntity = new Images($"{baseUrl}/Images/{id}.png", idChapter);
        bool result = await _repositoryImages.AddAsync(imageEntity);

        if (!result)
            return StatusCode(500, "An error occurred while saving the image.");

        return Ok(new { imageUrl = imageEntity.Url });
    }
}
