using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlockApp.Api.Data;

// Used by: dotnet ef migrations add / dotnet ef database update (local dev)
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=app/data/blockapp.db");
        return new AppDbContext(optionsBuilder.Options);
    }
}
