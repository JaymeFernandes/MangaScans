using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Mappings;

public static class Chapters
{
    public static void MapChapters(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chapter>(x =>
        {
            x.HasKey(x => x.Id);
            
            x.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            x.HasOne(c => c._Manga)
                .WithMany(c => c._Chapters)
                .HasForeignKey(c => c.IdManga)
                .OnDelete(DeleteBehavior.NoAction);
        });


    }
}