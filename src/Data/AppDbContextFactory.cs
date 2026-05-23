using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PwnLearn.Data;

/// <summary>Creates the portable LocalDB context for EF Core command-line tooling.</summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var projectDirectory = Directory.GetCurrentDirectory();
        var dataDirectory = Path.GetFullPath(Path.Combine(projectDirectory, "..", "data"));
        Directory.CreateDirectory(dataDirectory);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(projectDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?.Replace("|DataDirectory|", dataDirectory, StringComparison.Ordinal)
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AppDbContext(options);
    }
}
