using System.Text.Json.Serialization;
using MangaScans.Domain.Entities.Shared;

namespace MangaScans.Domain.Entities;

public class Category : Entity
{
    public virtual ICollection<Manga> Mangas { get; set; }
    
    public Category(int id, string name) : base(id, name) { }
    public Category(string name) : base(0, name) { }
}