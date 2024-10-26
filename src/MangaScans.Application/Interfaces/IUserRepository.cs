using MangaScans.Application.DTOs.Response.User;

namespace MangaScans.Identity.Interfaces;

public interface IUserRepository
{
    public Task<UserActionResponse> AddLikeManga(string userId, string mangaId);
    public Task<UserActionResponse> RemoveLikeManga(string userId, string mangaId);

    public Task<UserActionResponse> AddFavoriteUser(string userId, string mangaId);
    public Task<UserActionResponse> RemoveFavoriteUser(string userId, string MangaId);
    public Task<FavorityResponse> GetFavoriteManga(string userId);
    
    public Task<UserActionResponse>  AddHistoryManga(string userId, string mangaId, int num);
}

