namespace MangaScans.Domain.Entities.Shered;

public class BaseUserInfo
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    
    public virtual User User { get; set; }

    public BaseUserInfo(int id, string userId)
    {
        Id = id;
        UserId = userId;
    }
}