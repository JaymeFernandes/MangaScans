using MangaScans.Domain.Entities.Shered;

namespace MangaScans.Domain.Entities;

public class History : BaseUserInfo
{
    
    public string MangaId { get; set; }
    public int ChapterId { get; set; }
    
    public DateTime DateRead { get; set; }

    public History(int id, string userId, DateTime dateRead, string mangaId, int chapterId) : base(id, userId)
    {
        DateRead = dateRead;
        MangaId = mangaId;
        ChapterId = chapterId;
    }
    
    public History(string userId, DateTime dateRead, string mangaId, int chapterId) : this(0, userId, dateRead, mangaId, chapterId) { }
}