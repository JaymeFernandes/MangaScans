using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Mappings;

public static class Categories
{
    public static void MapCategories(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            
            entity.HasKey(x => x.Id);
            
            entity.Property(x => x.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);
                

            entity.HasData(new[]
            {
                new Category(1, "Action"),
                new Category(2, "Adventure"),
                new Category(3, "Comedy"),
                new Category(4, "Drama"),
                new Category(5, "Romance"),
                new Category(6, "Mystery"),
                new Category(7, "Suspense"),
                new Category(8, "Fantasy"),
                new Category(9, "Sci-Fi"),
                new Category(10, "Horror"),
                new Category(11, "Slice of Life"),
                new Category(12, "Supernatural"),
                new Category(13, "Historical"),
                new Category(14, "Sports"),
                new Category(15, "Harem"),
                new Category(16, "Yaoi"),
                new Category(17, "Yuri"),
                new Category(18, "Isekai")
            });
        });
    }
}