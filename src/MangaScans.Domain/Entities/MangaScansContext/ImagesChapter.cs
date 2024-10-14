using System.Text.Json.Serialization;
using MangaScans.Domain.Entities.Shared;

namespace MangaScans.Domain.Entities;

public class ImagesChapter
{
    public int Id { get; set; }
    public int IdChapter { get; set; }
    public string Url { get; set; }
    
    public int Sequence { get; set; }
    
    [JsonIgnore]
    public virtual Chapter _Chapter { get; set; }
    
    public ImagesChapter(int id, string urlCode, int idChapter)
    {
        Id = id;
        IdChapter = idChapter;
        Url = urlCode;
    }
    
    public ImagesChapter(string url, int idChapter) : this(0,  url, idChapter) { }
    
    public ImagesChapter(int id, string url, int idChapter, int sequence) : this(id, url, idChapter)
        => Sequence = sequence;
    
    public ImagesChapter(string url, int idChapter, int sequence) : this(0, url, idChapter, sequence) { }

}