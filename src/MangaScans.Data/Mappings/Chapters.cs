using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScans.Data.Mappings;

public class ChaptersMap : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.HasKey(x => x.Id);
            
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.HasOne(c => c._Manga)
            .WithMany(c => c.Chapters)
            .HasForeignKey(c => c.IdManga)
            .OnDelete(DeleteBehavior.NoAction);
    }
}