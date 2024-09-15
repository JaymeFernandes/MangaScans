using MangaScans.Domain.Entities;

namespace MangaScans.Api;

public class MangaDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int IdCategory { get; set; }
    
    public ICollection<Chapter> ChaptersSave { get; set; }
}