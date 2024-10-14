using MangaScans.Domain.Entities.Shered;

namespace MangaScans.Domain.Entities;

public class Favorite : BaseUserInfo
{
    public string MangaId { get; set; }
    
    public Favorite(int id, string userId, string mangaId) : base(id, userId)
        => MangaId = mangaId;
    
    public Favorite(string userId, string mangaId) : this(0, userId, mangaId) { }
}