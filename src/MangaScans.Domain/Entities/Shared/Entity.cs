namespace MangaScans.Domain.Entities.Shared;

public class Entity
{
    public int Id { get; init; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public Entity(int id, string name)
    {
        Id = id;
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }
}