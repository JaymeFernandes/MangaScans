using MangaScans.Domain.Entities;
using MangaScans.Identity.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Identity.Context;

public class AppIdentityDbContext : IdentityDbContext<User>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }
    
    public virtual DbSet<History> Histories { get; set; }
    public virtual DbSet<Like> Likes { get; set; }
    public virtual DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new MapFavorite());
        builder.ApplyConfiguration(new MapLike());
        builder.ApplyConfiguration(new MapUser());
        
        base.OnModelCreating(builder);
    }
}