using System.Text.Json.Serialization;
using MangaScans.Domain.Entities.Shared;

namespace MangaScans.Domain.Entities;

public class Images
{
    public int Id { get; set; }
    public int IdChapter { get; set; }
    public string Url { get; set; }
    
    public int Sequence { get; set; }
    
    [JsonIgnore]
    public virtual Chapter _Chapter { get; set; }
    
    public Images(int id, string urlCode, int idChapter)
    {
        Id = id;
        IdChapter = idChapter;
        Url = urlCode;
    }
    
    public Images(string url, int idChapter) : this(0,  url, idChapter) { }
    
    public Images(int id, string url, int idChapter, int sequence) : this(id, url, idChapter)
        => Sequence = sequence;
    
    public Images(string url, int idChapter, int sequence) : this(0, url, idChapter, sequence) { }

}