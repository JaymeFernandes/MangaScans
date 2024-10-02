using System.Net.Mime;
using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScans.Data.Mappings;

public class ImageMap : IEntityTypeConfiguration<Images>
{
    public void Configure(EntityTypeBuilder<Images> builder)
    {
        builder.HasKey(x => x.Id);
            
        builder.Property(x => x.Url)
            .IsRequired()
            .IsUnicode(true);

        builder.HasOne(x => x._Chapter)
            .WithMany(x => x._Images)
            .HasForeignKey(x => x.IdChapter);
    }
}