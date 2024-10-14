using MangaScans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScans.Identity.Mappings;

public class MapLike : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.MangaId)
            .IsRequired();
        
        builder.Property(x => x.MangaId)
            .IsRequired();
    }
}