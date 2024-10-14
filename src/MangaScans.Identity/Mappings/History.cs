using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScans.Identity.Mappings;

public class MapHistory : IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.MangaId)
            .IsRequired();
        
        builder.Property(x => x.ChapterId)
            .IsRequired();

        builder.Property(x => x.DateRead)
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .IsRequired();
    }
}