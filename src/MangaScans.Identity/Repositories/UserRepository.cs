using MangaScans.Application.DTOs.Response.Public_Routes;
using MangaScans.Application.DTOs.Response.User;
using MangaScans.Application.Interfaces;
using MangaScans.Data.Context;
using MangaScans.Domain.Entities;
using MangaScans.Identity.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Identity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly AppIdentityDbContext _appIdentityDbContext;
    private readonly AppDbContext _dbContext;

    public UserRepository(UserManager<User> userManager, AppDbContext appDbContext, AppIdentityDbContext appIdentityDbContext)
    {
        _userManager = userManager;
        _dbContext = appDbContext;
        _appIdentityDbContext = appIdentityDbContext;
    }
    
    public async Task<UserActionResponse> AddLikeManga(string userId, string mangaId)
    {
        var user = await _userManager.Users
            .Include(u => u.Like)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var manga = await _dbContext.Mangas
            .FirstOrDefaultAsync(x => x.Id == mangaId);
        
        if (manga == null || user == null)
        {
            var response = new UserActionResponse(false);
            if (user == null) response.AddError("User not found");
            if (manga == null) response.AddError("Manga not found");
            return response;
        }

        if (user.Like.Any(x => x.MangaId == mangaId))
            return new(false)
            { Errors = new() { "User already liked this manga" } };

        
        var like = new Like(userId, mangaId); 
        
        await _appIdentityDbContext.Likes.AddAsync(like);
        manga.Likes++;

        _dbContext.Mangas.Update(manga);
            
        await _dbContext.SaveChangesAsync();
        await _appIdentityDbContext.SaveChangesAsync();
            
        return new(true);
    }

    public async Task<UserActionResponse> RemoveLikeManga(string userId, string mangaId)
    {
        var user = await _userManager.Users
            .Include(u => u.Like)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var manga = await _dbContext.Mangas
            .FirstOrDefaultAsync(x => x.Id == mangaId);
        
        var responseFailed = new UserActionResponse(false);

        if (manga == null || user == null)
        {
            if (user == null) responseFailed.AddError("User not found");
            if (manga == null) responseFailed.AddError("Manga not found");
            
            return responseFailed;
        }

        if (user.Like.Any(x => x.MangaId == mangaId))
        {
            var like = user.Like.FirstOrDefault(x => x.MangaId == mangaId);

            manga.Likes--;

            if (like != null) _appIdentityDbContext.Likes.Remove(like);

            _dbContext.Mangas.Update(manga);
            
            await _dbContext.SaveChangesAsync();
            await _appIdentityDbContext.SaveChangesAsync();
        }
            
        responseFailed.AddError("User does not have a like to remove");

        return responseFailed;
    }

    public async Task<bool> IsLikeManga(string userId, string mangaId)
    {
        var result = await _appIdentityDbContext.Likes.FirstOrDefaultAsync(x => x.UserId == userId && x.MangaId == mangaId);
        
        return result != null;
    }

    public async Task<UserActionResponse> AddFavoriteUser(string userId, string mangaId)
    {
        var user = await _userManager.Users
            .Include(u => u.Favorite)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            return new(false) { Errors = new List<string>() { "User not found" } };

        if (user.Favorite.Any(x => x.MangaId == mangaId))
            return new(false) { Errors = new List<string>() { "This manga is already in your favorites" } };
        
        var favorite = new Favorite(userId, mangaId);
        
        await _appIdentityDbContext.Favorites.AddAsync(favorite);
        await _appIdentityDbContext.SaveChangesAsync();

        return new(true);
    }

    public async Task<UserActionResponse> RemoveFavoriteUser(string userId, string mangaId)
    {
        var user = _userManager.Users
            .Include(x => x.Favorite)
            .FirstOrDefault(x => x.Id == userId);
        
        if (user == null)
            return new(false) { Errors = new List<string>() { "User not found" } };
        

        if (user.Favorite.Any(x => x.MangaId == mangaId))
        {
            var entity = user.Favorite.FirstOrDefault(x => x.MangaId == mangaId);

            if (entity != null)
            {
                _appIdentityDbContext.Favorites.Remove(entity);
                await _appIdentityDbContext.SaveChangesAsync();
            }
            
            return new(true);
        }
        
        return new(false)
        {
            Errors = new() { "User does not have a favorite to remove" }
        };
            
    }

    public async Task<bool> IsFavoriteManga(string userId, string mangaId)
    {
        var result = await _appIdentityDbContext.Favorites.FirstOrDefaultAsync(x => x.MangaId == mangaId && x.UserId == userId);
        
        return result != null;
    }

    public async Task<FavorityResponse> GetFavoriteManga(string userId)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Include(x => x.Favorite)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null || !user.Favorite.Any())
            return new()
            {
                Mangas = new List<MangaDtoResponse>()
            };
        
        string[] favorites = user.Favorite.Select(x => x.MangaId).ToArray();

        var mangas = await _dbContext.Mangas
            .AsNoTracking()
            .Where(x => favorites.Contains(x.Id))
            .ToListAsync();

        return new()
        {
            Mangas = mangas.TolibraryResponse()
        };
    }

    public async Task<UserActionResponse> AddHistoryManga(string userId, string mangaId, int num)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            History history = new(userId, DateTime.UtcNow, mangaId, num);
            
            await _appIdentityDbContext.Histories.AddAsync(history);
            
            await _appIdentityDbContext.SaveChangesAsync();
            
            return new(true);
        }

        return new(false);
    }
    
}