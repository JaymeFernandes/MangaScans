namespace MangaScans.Domain.Entities;

public class ImagesCover
{
    public int Id { get; set; }
    
    public string Link { get; set; }
    
    public string MangaId { get; set; }
    
    public virtual Manga Manga { get; set; }

    public ImagesCover(string link)
    {
        Link = link;
    }
}