using MangaScans.Api.Controllers.Shared;
using MangaScans.Application.DTOs.Request;
using MangaScans.Application.DTOs.Response;
using MangaScans.Data.Context;
using MangaScans.Data.Exceptions;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Api.Controllers;

[Tags("Private Routes (Admin)", "Mangas")]
[Route("api/manga")]
public class AdminMangaController : CustomControllerBase
{
    protected readonly IRepositoryManga _manga;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminMangaController([FromServices] IRepositoryManga manga, [FromServices] IHttpContextAccessor httpContext)
    {
        _manga = manga;
        _httpContextAccessor = httpContext;
    }
    
    /// <summary>
    /// Retorna todos os mangas do banco de dados.
    /// </summary>
    /// <returns>Uma lista de todos os mangas disponíveis.</returns>
    /// <response code="200">Retorna a lista de mangas.</response>
    /// <response code="404">Lança uma exceção se não houver mangas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(MangaDtoRequest), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> GetAllAsync()
    {
        var mangas = await _manga.GetAll();

        if (mangas.Count == 0) 
            throw new DbEntityException("there are no mangas at the moment");
        
        return Ok(mangas);
    }
    
    /// <summary>
    /// Adiciona um novo manga ao banco de dados.
    /// </summary>
    /// <param name="manga">Objeto DTO que contém os dados do manga a ser adicionado.</param>
    /// <returns>Retorna o manga recém-adicionado.</returns>
    /// <response code="201">Manga adicionado com sucesso.</response>
    /// <response code="401">Lança a  exeção caso não consiga adicionar o manga</response>
    [HttpPost]
    [ProducesResponseType(typeof(MangaDtoRequest), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AddAsync([FromBody] MangaDtoRequest manga)
    {
        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        
        var entity = new Manga(manga.Category, manga.Name, manga.Description);
        
        var response = await _manga.AddAsync(entity);

        if (!response) 
            throw new DbEntityException("There was an error adding the manga");

        return Created($"{baseUrl}/api/{entity.Id}/", entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromBody] MangaDtoRequest manga, [FromRoute] string id)
    {
        var result =await _manga.UpdateAsync(new Manga(manga.Category, manga.Name, manga.Description), id);

        if (!result) return BadRequest();
        
        return Ok();
    }
    
}