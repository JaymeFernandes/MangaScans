using System.Security.Claims;
using MangaScans.Application.DTOs.Request.User;
using MangaScans.Application.DTOs.Response.User;
using MangaScans.Application.Services;
using MangaScans.Identity.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Users;

/// <summary>
/// AccessAccountController handles account-related actions, such as login, registration, 
/// and refreshing login tokens for authenticated users.
/// </summary>
[Tags("Access")]
public class AccessAccountController : ControllerBase
{
    private readonly IIdentityServices _identityServices;
    
    /// <summary>
    /// Initializes a new instance of the AccessAccountController class with injected identity services.
    /// </summary>
    /// <param name="identityServices">Injected service handling identity operations like login and registration.</param>
    public AccessAccountController(IIdentityServices identityServices)
        => _identityServices = identityServices;

    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="request">Login request containing the user's credentials.</param>
    /// <returns>A response indicating whether authentication was successful.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginDtoResponse>> Login([FromBody] LoginDtoRequest request)
    {

        if ((bool)HttpContext.User.Identity?.IsAuthenticated)
            return BadRequest();

        var result = await _identityServices.LoginAsync(request);

        if (result.IsAuthenticated)
            return Ok(result);
 
        return Unauthorized(result);
    }

    /// <summary>
    /// Registers a new user with the provided information.
    /// </summary>
    /// <param name="request">Registration request containing the user's details.</param>
    /// <returns>A response indicating whether registration was successful.</returns>
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDtoRequest request)
    {
        if ((bool)HttpContext.User.Identity?.IsAuthenticated)
            return BadRequest();
        
        var result = await _identityServices.RegisterAsync(request);

        if (result.IsSuccess)
            return Ok(result);
      
        return BadRequest(result);
    }
    
    /// <summary>
    /// Refreshes the user's login session using a refresh token. Requires the "Refresh" role.
    /// </summary>
    /// <param name="address">Client's IP address or another identifying address.</param>
    /// <returns>A new authentication token if refresh is successful.</returns>
    [Authorize(Roles = Roles.Refresh)]
    [HttpPost("refresh-login")]
    public async Task<ActionResult<LoginDtoResponse>> RefreshLogin([FromHeader] string address)
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
