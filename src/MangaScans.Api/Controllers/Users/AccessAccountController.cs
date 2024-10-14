using System.Security.Claims;
using MangaScans.Application.DTOs.Request.User;
using MangaScans.Application.DTOs.Response.User;
using MangaScans.Application.Services;
using MangaScans.Identity.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Users;

[Tags("Access")]
public class AccessAccountController : ControllerBase
{
    protected readonly IIdentityServices _identityServices;
    
    public AccessAccountController(IIdentityServices identityServices)
        => _identityServices = identityServices;

    [HttpPost("login")]
    public async Task<ActionResult<LoginDtoResponse>> Login([FromBody] LoginDtoRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _identityServices.LoginAsync(request);

        if (result.IsAuthenticated)
            return Ok(result);
            
        return Unauthorized(result);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDtoRequest request)
    {
        var result = await _identityServices.RegisterAsync(request);

        if (result.IsSuccess)
            return Ok(result);
        
        return BadRequest(result);
    }
    
    [Authorize(Roles = Roles.Refresh)]
    [HttpPost("refresh-login")]
    public async Task<IActionResult> RefreshLogin([FromBody] string address)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return BadRequest();
        
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
        
        var result = await _identityServices.RefreshLoginAsync(userId, token, address);

        if (result.IsAuthenticated)
            return Ok(result);

        return Unauthorized();
    }
}