
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlockApp.Shared.Entities;

namespace BlockApp.Api.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.PhoneNumber)
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.LastLoginAt)
                .IsRequired(false);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}