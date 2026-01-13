using Application.Repositories;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// Contiene métodos de extensión para registrar los servicios de la capa
    /// Infrastructure en el contenedor de inyección de dependencias.
    /// </summary>
    /// <remarks>
    /// Centraliza la configuración de dependencias relacionadas con:
    /// <list type="bullet">
    /// <item>Acceso a datos (Entity Framework Core).</item>
    /// <item>Repositorios.</item>
    /// <item>Unidad de trabajo (transacciones).</item>
    /// <item>Servicios (multi-tenant).</item>
    /// </list>
    /// </remarks>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra los servicios de infraestructura y persistencia en el contenedor
        /// de inyección de dependencias.
        /// </summary>
        /// <param name="services">
        /// Colección de servicios sobre la cual se registran las dependencias.
        /// </param>
        /// <param name="connectionString">
        /// Cadena de conexión utilizada por <see cref="AppDbContext"/> para acceder
        /// a la base de datos SQL Server.
        /// </param>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(connectionString)
            );

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
