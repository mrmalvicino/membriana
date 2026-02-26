using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var dbConnectionString = configuration.GetConnectionString();

        if (dbConnectionString == null)
        {
            throw new InvalidOperationException("No existe el ConnectionString.");
        }

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(dbConnectionString)
            .Options;

        return new AppDbContext(options);
    }
}