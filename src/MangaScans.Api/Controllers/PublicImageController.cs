using MangaScans.Api.Controllers.Shared;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers;

[Route("api/images/library")]
public class PublicImageController : CustomControllerBase
{
    protected readonly IRepositoryImages _repositoryImages;
    
    public PublicImageController([FromServices] IRepositoryImages repositoryImages) 
        => _repositoryImages = repositoryImages;
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetImageById([FromRoute] int id)
    {
        try
        {
            var path = await _repositoryImages.GetUrlById(id);

            if (string.IsNullOrEmpty(path))
                return NotFound();

            return Redirect(path);
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }
    
}