using MangaScans.Data.Mappings;
using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.MapCategories();
        modelBuilder.MapChapters();
        modelBuilder.MapImage();
        modelBuilder.MapMangas();
        
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Chapter> Chapters { get; set; }
    public virtual DbSet<Images> Images { get; set; }
    public virtual DbSet<Manga> Mangas { get; set; }
}