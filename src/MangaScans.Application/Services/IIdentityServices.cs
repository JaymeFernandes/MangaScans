using MangaScans.Application.DTOs.Request.User;
using MangaScans.Application.DTOs.Response.User;

namespace MangaScans.Application.Services;

public interface IIdentityServices
{

    public Task<LoginDtoResponse> RefreshLoginAsync(string userId, string token, string address);
    
    public Task<RegisterDtoResponse> RegisterAsync(RegisterDtoRequest request);
    
    public Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request);
}