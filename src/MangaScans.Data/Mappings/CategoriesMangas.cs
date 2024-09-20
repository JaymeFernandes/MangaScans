using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Mappings;

public static class CategoriesMangas
{
    public static void MapCategoriesMangas(this ModelBuilder builder)
    {
        builder.Entity<CategoryManga>(x =>
        {
            x.HasKey(x => x.Id);

            x.HasOne(x => x.Category)
                .WithMany(x => x.CategoryMangas)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            x.HasOne(x => x.Manga)
                .WithMany(x => x.CategoryMangas)
                .HasForeignKey(x => x.MangaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}