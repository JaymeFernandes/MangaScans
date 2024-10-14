using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace MangaScans.Domain.Entities;

public class Manga
{
    public string Id { get; set; }
    
    public int Views { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ImagesCover Cover { get; set; }
    public virtual List<Category> Categories { get; set; }
    public virtual ICollection<CategoryManga> CategoryMangas { get; set; }
    public virtual ICollection<Chapter> Chapters { get; set; }

    public Manga(string id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = DateTime.Now;
    }

    public Manga(string name, string description) : this(GenerateId(), name, description) { }

    private static string GenerateId()
    {
        string timestamp = DateTime.UtcNow.ToString("yyMMddHHMMss");
        
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBytes = new byte[4];
            rng.GetBytes(randomBytes);
            string randomValue = BitConverter.ToUInt32(randomBytes, 0).ToString("X6");

            return timestamp + randomValue;
        }
    }
    
}