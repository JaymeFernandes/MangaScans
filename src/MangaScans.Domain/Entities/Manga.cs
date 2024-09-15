using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace MangaScans.Domain.Entities;

public class Manga
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int IdCategory { get; set; }
    
    [JsonIgnore]
    public virtual Category _Categories { get; set; }
    
    public virtual ICollection<Chapter> _Chapters { get; set; }

    public Manga(string id, int idCategory, string name, string description)
    {
        Id = id;
        Name = name;
        IdCategory = idCategory;
        Description = description;
        CreatedAt = DateTime.Now;
    }

    public Manga(int IdCategoty, string name, string description) : this(GenerateId(), IdCategoty, name, description) { }

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