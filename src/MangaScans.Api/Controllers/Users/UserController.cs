using System.Security.Claims;
using MangaScans.Api.Controllers.Shared;
using MangaScans.Application.DTOs.Response.User;
using MangaScans.Identity.Consts;
using MangaScans.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Users;

[Tags("User Function")]
[Authorize(Roles = Roles.User)]
public class UserController :  BaseController
{
    protected readonly IUserRepository _userRepository;
    
    public UserController(IUserRepository userRepository)
        => _userRepository = userRepository;

    [HttpPost("Like")]
    public async Task<ActionResult<UserActionResponse>> LikeManga([FromHeader] string mangaId)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var identity =  HttpContext.User.Identity as ClaimsIdentity;
        var userId =  identity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var result = await _userRepository.AddLikeManga(userId, mangaId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("Unlike")]
    public async Task<ActionResult<UserActionResponse>> UnlikeManga([FromHeader] string mangaId)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var identity =  HttpContext.User.Identity as ClaimsIdentity;
        var userId =  identity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var result = await _userRepository.RemoveLikeManga(userId, mangaId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("Favorite")]
    public async Task<ActionResult<UserActionResponse>> AddFavoriteManga([FromHeader] string mangaId)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var identity =  HttpContext.User.Identity as ClaimsIdentity;
        var userId =  identity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var result = await _userRepository.AddFavoriteUser(userId, mangaId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("UnFavorite")]
    public async Task<ActionResult<UserActionResponse>> RemoveFavoriteManga([FromHeader] string mangaId)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var identity =  HttpContext.User.Identity as ClaimsIdentity;
        var userId =  identity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var result = await _userRepository.RemoveFavoriteUser(userId, mangaId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("Favorite")]
    public async Task<ActionResult<FavorityResponse>> GetFavoriteManga()
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var identity =  HttpContext.User.Identity as ClaimsIdentity;
        var userId =  identity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var result = await _userRepository.GetFavoriteManga(userId);

        if (result.Mangas.Count == 0)
            return NotFound();
        
        return Ok(result);
    }
}