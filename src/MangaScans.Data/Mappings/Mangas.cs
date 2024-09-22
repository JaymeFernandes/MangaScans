using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScans.Data.Mappings;

public class MangasMap : IEntityTypeConfiguration<Manga>
{
    public void Configure(EntityTypeBuilder<Manga> builder)
    {
        builder.HasKey(x => x.Id);
            
        builder.Property(c => c.Views)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.Likes)
            .HasDefaultValue(0)
            .IsRequired();
            
        builder.Property(x => x.Dislikes)
            .HasDefaultValue(0)
            .IsRequired();
            
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(x => x.Description)
            .IsRequired()
            .IsUnicode(false);

        builder.HasMany(m => m.Categories)
            .WithMany(c => c.Mangas)
            .UsingEntity<CategoryManga>();
    }
}
