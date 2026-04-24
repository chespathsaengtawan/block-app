using BlockApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using BlockApp.Api.Data.Configurations;

namespace BlockApp.Api.Data
{

    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
        public DbSet<BlockEntry> BlockEntries => Set<BlockEntry>();
        public DbSet<UserBlockEntry> UserBlockEntries => Set<UserBlockEntry>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PointTransaction> PointTransactions => Set<PointTransaction>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<RewardActivity> RewardActivities => Set<RewardActivity>();
        public DbSet<PointsPackage> PointsPackages => Set<PointsPackage>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OtpCodeConfiguration());
            modelBuilder.ApplyConfiguration(new BlockEntryConfiguration());
            modelBuilder.ApplyConfiguration(new UserBlockEntryConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

            // Seed default points packages
            modelBuilder.Entity<PointsPackage>().HasData(
                new PointsPackage 
                { 
                    Id = 1, 
                    Points = 100, 
                    PriceTHB = 100, 
                    BonusPoints = 0, 
                    DisplayOrder = 1, 
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new PointsPackage 
                { 
                    Id = 2, 
                    Points = 500, 
                    PriceTHB = 500, 
                    BonusPoints = 50, 
                    DisplayOrder = 2, 
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new PointsPackage 
                { 
                    Id = 3, 
                    Points = 1000, 
                    PriceTHB = 1000, 
                    BonusPoints = 150, 
                    DisplayOrder = 3, 
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new PointsPackage 
                { 
                    Id = 4, 
                    Points = 5000, 
                    PriceTHB = 5000, 
                    BonusPoints = 1000, 
                    DisplayOrder = 4, 
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }

}