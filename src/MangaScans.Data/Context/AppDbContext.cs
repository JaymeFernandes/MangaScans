using MangaScans.Data.Mappings;
using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoriesMangasMap());
        modelBuilder.ApplyConfiguration(new CategoriesMap());
        modelBuilder.ApplyConfiguration(new CategoriesMap());
        modelBuilder.ApplyConfiguration(new ImageMap());
        modelBuilder.ApplyConfiguration(new MangasMap());
        
        base.OnModelCreating(modelBuilder);
    }

    
    public virtual DbSet<Chapter> Chapters { get; set; }
    public virtual DbSet<Images> Images { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Manga> Mangas { get; set; }
    public virtual DbSet<CategoryManga> CategorysMangas { get; set; }
}