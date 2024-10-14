
using Microsoft.AspNetCore.Identity;

namespace MangaScans.Domain.Entities;

public class User : IdentityUser
{
    public virtual ICollection<Favorite> Favorite { get; set; }
    public virtual ICollection<History> History { get; set; }
    public virtual ICollection<Like> Like { get; set; }
}