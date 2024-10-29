using MangaScans.Api.Controllers.Shared;
using MangaScans.Api.Services;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Admin;

/// <summary>
/// ImageController manages administrative operations related to chapter images.
/// </summary>
[Route("api/admin/images")]
[Tags("images")]
public class ImageController : AdminBaseController
{
    private readonly IRepositoryImages _repositoryImages;

    /// <summary>
    /// Initializes a new instance of the ImageController with the image repository service.
    /// </summary>
    /// <param name="repositoryImages">Injected service for managing image data operations.</param>
    public ImageController(IRepositoryImages repositoryImages)
    {
        _repositoryImages = repositoryImages;
    }

    /// <summary>
    /// Retrieves all images stored in the system.
    /// </summary>
    /// <returns>A list of all images.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllImagesAsync()
        => Ok(await _repositoryImages.GetAll());

    /// <summary>
    /// Retrieves a specific image by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the image.</param>
    /// <returns>The image with the specified ID, or a 404 status if not found.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageByIdAsync(int id)
    {
        var entity = await _repositoryImages.GetById(id);

        // Return 404 Not Found if the image does not exist.
        if (entity == null)
            return NotFound();

        return Ok(entity);
    }
    
    /// <summary>
    /// Uploads a new image associated with a specific chapter.
    /// </summary>
    /// <param name="idChapter">The unique identifier of the chapter.</param>
    /// <param name="file">The image file to be uploaded.</param>
    /// <returns>A response with the result status and URL of the saved image.</returns>
    [HttpPost("{idChapter}")]
    public async Task<IActionResult> UploadImageAsync([FromRoute] int idChapter, IFormFile file)
    {
        // Generate a unique URL path for the image based on the chapter ID.
        string path = await _repositoryImages.GenerateImageUrl(idChapter);
        
        // Create an instance of ChaptersImagesService to handle image saving operations.
        ChaptersImagesService image = new ChaptersImagesService(path, idChapter);
        var imageEntity = await image.SaveImageAsync(file);

        // Return BadRequest if the image could not be saved.
        if (imageEntity == null)
            return BadRequest();
        
        // Add the saved image entity to the repository.
        bool result = await _repositoryImages.AddAsync(imageEntity);

        // Return server error if image saving fails.
        if (!result)
            return StatusCode(500, "An error occurred while saving the image.");

        return Ok(new { result, UrlImage = imageEntity.Url });
    }

    /// <summary>
    /// Deletes an image by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the image to delete.</param>
    /// <returns>A response with the result status of the delete operation.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteImageAsync([FromRoute] int id)
    {
        // Attempt to delete the image by its ID.
        var result = await _repositoryImages.DeleteByIdAsync(id);

        // Return server error if deletion fails.
        if (!result)
            return StatusCode(500, "An error occurred while deleting the image.");
        
        return Ok();
    }
}
