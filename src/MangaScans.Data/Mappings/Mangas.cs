using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Data.Mappings;

public static class Mangas
{
    public static void MapMangas(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manga>(x =>
        {
            x.HasKey(x => x.Id);
            
            x.Property(c => c.Views)
                .HasDefaultValue(0)
                .IsRequired();

            x.Property(x => x.Likes)
                .HasDefaultValue(0)
                .IsRequired();
            
            x.Property(x => x.Dislikes)
                .HasDefaultValue(0)
                .IsRequired();
            
            x.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            x.Property(x => x.Description)
                .IsRequired()
                .IsUnicode(false);

            x.HasOne(x => x._Categories)
                .WithMany(x => x.Mangas)
                .HasForeignKey(x => x.IdCategory);

            


        });
    }
}
