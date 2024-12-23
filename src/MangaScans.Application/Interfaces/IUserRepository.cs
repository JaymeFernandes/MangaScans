using MangaScans.Application.DTOs.Response.User;

namespace MangaScans.Application.Interfaces;

public interface IUserRepository
{
    public Task<UserActionResponse> AddLikeManga(string userId, string mangaId);
    public Task<UserActionResponse> RemoveLikeManga(string userId, string mangaId);

    public Task<bool> IsLikeManga(string userId, string mangaId);

    public Task<UserActionResponse> AddFavoriteUser(string userId, string mangaId);
    public Task<UserActionResponse> RemoveFavoriteUser(string userId, string mangaId);
    public Task<bool> IsFavoriteManga(string userId, string mangaId);
    public Task<FavorityResponse> GetFavoriteManga(string userId);
    
    public Task<UserActionResponse>  AddHistoryManga(string userId, string mangaId, int num);
}

