
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlockApp.Shared.Entities;

namespace BlockApp.Api.Data.Configurations;

public class BlockNumberConfiguration : IEntityTypeConfiguration<BlockNumber>
{
    public void Configure(EntityTypeBuilder<BlockNumber> builder)
    {
        builder.ToTable("BlockNumbers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Note)
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.PhoneNumber })
            .IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
