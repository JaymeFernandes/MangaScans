using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScans.Data.Mappings;

public class CategoriesMangasMap : IEntityTypeConfiguration<CategoryManga>
{
    public void Configure(EntityTypeBuilder<CategoryManga> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.CategoryMangas)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Manga)
            .WithMany(x => x.CategoryMangas)
            .HasForeignKey(x => x.MangaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}