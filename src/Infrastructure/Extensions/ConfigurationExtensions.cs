using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

/// <summary>
/// Proporciona métodos de extensión para <see cref="IConfiguration"/>.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Obtiene una cadena de conexión de la configuración y valida que no sea nula o vacía.
    /// </summary>
    /// <param name="name">El nombre de la cadena de conexión. Por defecto es "DefaultConnection".</param>
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