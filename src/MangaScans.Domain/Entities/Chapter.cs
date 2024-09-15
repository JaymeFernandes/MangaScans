using System.Net.Mime;
using System.Text.Json.Serialization;
using MangaScans.Domain.Entities.Shared;

namespace MangaScans.Domain.Entities;

public class Chapter : Entity
{
    public string IdManga { get; set; }
    public int Num { get; set; } 
    
    [JsonIgnore]
    public virtual Manga _Manga { get; set; }
    
    public virtual ICollection<Images> _Images { get; set; }
    

    public Chapter(int id, string idManga, string name, int num) :  base(id, name)
    {
        IdManga = idManga;
        Num = num;
    }
    
    public Chapter(string idManga, string name, int num) : this(0, idManga, name, num) { }
}