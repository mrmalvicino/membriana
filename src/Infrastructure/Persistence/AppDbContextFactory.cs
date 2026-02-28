using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

/// <summary>
/// Fábrica para crear instancias de <see cref="AppDbContext"/> en tiempo de diseño.
/// Implementa <see cref="IDesignTimeDbContextFactory{TContext}"/> para permitir que las
/// herramientas de Entity Framework Core (como migraciones) creen instancias del contexto
/// de base de datos sin necesidad de ejecutar la aplicación completa.
/// </summary>
/// <remarks>
/// Esta clase es necesaria para poder modularizar la configuración del <see cref="AppDbContext"/>
/// mediante archivos de configuración separados (utilizando <see cref="IEntityTypeConfiguration{TEntity}"/>),
/// instanciando el contexto para poder descubrir y aplicar las configuraciones al generar migraciones.
/// </remarks>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="AppDbContext"/> configurada para tiempo de diseño.
    /// </summary>
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