using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScans.Data.Mappings;

public class ImagesCoverMap : IEntityTypeConfiguration<ImagesCover>
{
    public void Configure(EntityTypeBuilder<ImagesCover> builder)
    {
        builder.ToTable("ImagesCover");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Link)
            .IsRequired()
            .IsUnicode();
        
        builder.HasOne(x => x.Manga)
            .WithOne(x => x.Cover)
            .HasForeignKey<ImagesCover>(x => x.MangaId);
    }
}