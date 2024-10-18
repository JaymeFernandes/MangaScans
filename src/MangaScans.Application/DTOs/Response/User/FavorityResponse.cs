using MangaScans.Application.DTOs.Response.Public_Routes;

namespace MangaScans.Application.DTOs.Response.User;

public class FavorityResponse
{
    public ICollection<MangaDtoResponse> Mangas { get; set; }
}