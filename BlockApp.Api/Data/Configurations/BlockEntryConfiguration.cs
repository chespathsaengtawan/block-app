using BlockApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockApp.Api.Data.Configurations;

public class BlockEntryConfiguration : IEntityTypeConfiguration<BlockEntry>
{
    public void Configure(EntityTypeBuilder<BlockEntry> builder)
    {
        builder.ToTable("BlockEntries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntryType).IsRequired();

        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
        builder.Property(x => x.BankName).HasMaxLength(100);
        builder.Property(x => x.AccountNumber).HasMaxLength(50);
        builder.Property(x => x.AccountHolderName).HasMaxLength(150);

        builder.Property(x => x.CreatedAt).IsRequired();

        // Unique index per entry type
        builder.HasIndex(x => x.PhoneNumber)
            .IsUnique()
            .HasFilter($"\"{nameof(BlockEntry.PhoneNumber)}\" IS NOT NULL");

        builder.HasIndex(x => new { x.BankName, x.AccountNumber })
            .IsUnique()
            .HasFilter($"\"{nameof(BlockEntry.AccountNumber)}\" IS NOT NULL");

        builder.HasOne(x => x.AddedBy)
            .WithMany()
            .HasForeignKey(x => x.AddedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
