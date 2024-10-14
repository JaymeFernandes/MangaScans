using MangaScans.Domain.Entities.Shered;

namespace MangaScans.Domain.Entities;

public class Like : BaseUserInfo
{
    public string MangaId { get; set; }
    
    public Like(int id, string UserId, string mangaId) : base(id, UserId) 
        => MangaId = mangaId;
    
    public Like(string UserId, string mangaId) :  this(0, UserId, mangaId) { }
}