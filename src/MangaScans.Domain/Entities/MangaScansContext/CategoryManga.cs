namespace MangaScans.Domain.Entities;

public class CategoryManga
{
    public int Id { get; set; }
    
    public int CategoryId { get; set; }
    public string MangaId { get; set; }
    
    public Category Category { get; set; }
    public Manga Manga { get; set; }
    
}