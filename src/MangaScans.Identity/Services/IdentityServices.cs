using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MangaScans.Application.DTOs.Request.User;
using MangaScans.Application.DTOs.Response.User;
using MangaScans.Application.Services;
using MangaScans.Domain.Entities;
using MangaScans.Identity.Configuration;
using MangaScans.Identity.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MangaScans.Identity.Services;

public class IdentityServices : IIdentityServices
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JWTConfig _jwtConfig;

    public IdentityServices(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<JWTConfig> jwtConfig)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<LoginDtoResponse> RefreshLoginAsync(string userId, string token, string address)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new(false);
        
        var result = await _userManager.GetAuthenticationTokenAsync(user, "MangaScans", address);

        if (result == null)
            return new(false);
        
        if (result == token)
        {
            if (user.Email != null)
            {
                var usertoken = await GenerateTokensAsync(user.Email);
            
                var refreshToken = await _userManager.RemoveAuthenticationTokenAsync(user, "MangaScans", address);

                if (refreshToken.Succeeded)
                    await _userManager.SetAuthenticationTokenAsync(user, "MangaScans", address, usertoken.RefreshToken);

                return usertoken;
            }
        }

        return new(false);
    }
    
    public async Task<RegisterDtoResponse> RegisterAsync(RegisterDtoRequest request)
    {
        User user = new()
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };
        
        var result =  await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Roles.User);
            
            return new(true);
        }
            

        RegisterDtoResponse response = new(false);

        if (result.Errors.Count() > 0)
            response.AddErrors(result.Errors.Select(x => x.Description));

        return response;
    }

    public async Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);

        if (result.Succeeded)
        {
            
            var address = Guid.NewGuid().ToString();
            var token = await GenerateTokensAsync(request.Email);
            token.Address = address;
            
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
                await _userManager.SetAuthenticationTokenAsync(user, "MangaScans", address, token.RefreshToken);

            return token;
        }
            
        
        LoginDtoResponse response = new(false);
        

        if (result.IsLockedOut)
            response.AddErro("Account locked out");

        if (result.IsNotAllowed)
            response.AddErro("Accont is not allowed");
        
        return response;
    }

    private async Task<LoginDtoResponse> GenerateTokensAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return new(false);

        var claimsUser = await GetUserClaims(user, false);
        var claimsRefresh = await GetUserClaims(user, true);

        string token = await CreateToken(claimsUser.ToList());
        string refreshToken = await CreateToken(claimsRefresh.ToList(), true);

        return new(true, token, refreshToken);
    }

    private Task<string> CreateToken(List<Claim> claims, bool isRefreshToken = false)
    {
        var expires = isRefreshToken ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(5);

        var jwt = new JwtSecurityToken
        (
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: _jwtConfig.SigningCredentials
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    private async Task<IList<Claim>> GetUserClaims(User user, bool isRefreshToken = false)
    {
        List<Claim> claims = new();
        
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));

        if (!isRefreshToken)
        {
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim("role", role));
        }
        else
        {
            claims.Add(new Claim("role", Roles.Refresh));
        }

        return claims;
    }
}