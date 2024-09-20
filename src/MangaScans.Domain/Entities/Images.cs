using System.Text.Json.Serialization;
using MangaScans.Domain.Entities.Shared;

namespace MangaScans.Domain.Entities;

public class Images
{
    public int Id { get; set; }
    public int IdChapter { get; set; }
    public string Url { get; set; }
    
    [JsonIgnore]
    public virtual Chapter _Chapter { get; set; }
    
    public Images(int id, string urlCode, int idChapter)
    {
        Id = id;
        IdChapter = idChapter;
        Url = urlCode;
    }
    
    public Images(string url, int idChapter) : this(0,  url, idChapter) { }

}