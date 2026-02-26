using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static string GetConnectionString(
        this IConfiguration configuration,
        string name = "DefaultConnection"
    )
    {
        var dbConnectionString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(configuration, name);

        if (string.IsNullOrWhiteSpace(dbConnectionString))
        {
            throw new InvalidOperationException($"No existe el ConnectionString {name}.");
        }

        return dbConnectionString;
    }
}