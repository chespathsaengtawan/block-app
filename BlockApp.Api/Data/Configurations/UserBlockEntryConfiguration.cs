using BlockApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockApp.Api.Data.Configurations;

public class UserBlockEntryConfiguration : IEntityTypeConfiguration<UserBlockEntry>
{
    public void Configure(EntityTypeBuilder<UserBlockEntry> builder)
    {
        builder.ToTable("UserBlockEntries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Note).HasMaxLength(500);
        builder.Property(x => x.OtherReason).HasMaxLength(255);
        builder.Property(x => x.Reasons).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        // ผู้ใช้บล็อกรายการเดิมซ้ำไม่ได้
        builder.HasIndex(x => new { x.UserId, x.BlockEntryId }).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.BlockEntry)
            .WithMany(e => e.UserBlockEntries)
            .HasForeignKey(x => x.BlockEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
