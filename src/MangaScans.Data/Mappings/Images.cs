using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Mappings;

public static class Image
{
    public static void MapImage(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Images>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Url)
                .IsRequired()
                .IsUnicode(true);

            x
                .HasOne(x => x._Chapter)
                .WithMany(x => x._Images)
                .HasForeignKey(x => x.IdChapter);
        });
    }
}