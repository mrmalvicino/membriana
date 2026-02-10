using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// Representa el contexto de la base de datos para la aplicación.
    /// Hereda de IdentityDbContext para incluir la funcionalidad de autenticación y
    /// autorización proporcionada por ASP.NET Core Identity.
    /// Define las entidades y relaciones del modelo de datos, así como la configuración
    /// de la base de datos.
    /// </summary>
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        /// <summary>
        /// Constructor principal.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipPlan> MembershipPlans { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PricingPlan> PricingPlans { get; set; }

        /// <summary>
        /// Establece opciones adicionales, como la conexión a la base de datos y el proveedor
        /// de base de datos.
        /// </summary>
        /// <remarks>
        /// En este caso, se ignora la advertencia de cambios pendientes en el modelo, ya que
        /// se están utilizando datos de ejemplo (dummy data) y no se requiere una migración
        /// completa del modelo.
        /// </remarks>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)); // Dummy data
        }

        /// <summary>
        /// Método que se llama al crear el modelo de datos. Configura las entidades, relaciones,
        /// restricciones y tipos de datos específicos para la base de datos.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Tablas

            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<Member>().ToTable("Members");
            builder.Entity<Organization>().ToTable("Organizations");
            builder.Entity<Person>().ToTable("People");

            #endregion

            #region Relaciones 1 a 1

            builder.Entity<Organization>()
                .HasOne(o => o.LogoImage)
                .WithOne()
                .HasForeignKey<Organization>(o => o.LogoImageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Person>()
                .HasOne(p => p.ProfileImage)
                .WithOne()
                .HasForeignKey<Person>(p => p.ProfileImageId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Relaciones 1 a N

            builder.Entity<Organization>()
                .HasMany(o => o.Employees)
                .WithOne(e => e.Organization)
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Organization>()
                .HasMany(o => o.Members)
                .WithOne(m => m.Organization)
                .HasForeignKey(m => m.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Organization>()
                .HasMany(o => o.MembershipPlans)
                .WithOne(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Member>()
                .HasMany(p => p.Payments)
                .WithOne(m => m.Member)
                .HasForeignKey(m => m.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Member>()
                .HasOne(m => m.MembershipPlan)
                .WithMany(p => p.Members)
                .HasForeignKey(m => m.MembershipPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Tipos de datos SQL

            builder.Entity<MembershipPlan>()
                .Property(m => m.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<PricingPlan>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            #endregion

            #region Constraints

            builder.Entity<MembershipPlan>()
                .HasIndex(m => m.Name)
                .IsUnique();

            #endregion

            #region Seed data

            builder.Entity<PricingPlan>().HasData(
                new PricingPlan
                {
                    Id = (int)Domain.Enums.PricingPlan.Free,
                    Name = "Plan gratuito",
                    Amount = 0
                },
                new PricingPlan
                {
                    Id = (int)Domain.Enums.PricingPlan.Professional,
                    Name = "Plan profesional",
                    Amount = 20
                },
                new PricingPlan {
                    Id = (int)Domain.Enums.PricingPlan.Enterprise,
                    Name = "Plan empresarial",
                    Amount = 30
                }
            );

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ((int)Domain.Enums.AppRole.Admin).ToString(),
                    Name = Domain.Enums.AppRole.Admin.ToString(),
                    NormalizedName = Domain.Enums.AppRole.Admin.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = ((int)Domain.Enums.AppRole.Employee).ToString(),
                    Name = Domain.Enums.AppRole.Employee.ToString(),
                    NormalizedName = Domain.Enums.AppRole.Employee.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = ((int)Domain.Enums.AppRole.Member).ToString(),
                    Name = Domain.Enums.AppRole.Member.ToString(),
                    NormalizedName = Domain.Enums.AppRole.Member.ToString().ToUpper()
                }
            );

            #endregion

            #region Dummy Data

            builder.Entity<Image>().HasData(
                new Image { Id = 1, Url = "https://i.imgur.com/Cy1SqZy.png" }
            );

            builder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = 1,
                    Active = true,
                    Name = "Ftnes Gym",
                    Email = "ftnesgym@mail.com",
                    Phone = "1512345678",
                    LogoImageId = 1,
                    PricingPlanId = 1
                }
            );

            var adminUser = new AppUser
            {
                UserName = "admin@mail.com",
                Email = "admin@mail.com",
                NormalizedEmail = "admin@mail.com",
                EmailConfirmed = true,
                OrganizationId = 1
            };

            var hasher = new PasswordHasher<AppUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Password123-");
            builder.Entity<AppUser>().HasData(adminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminUser.Id,
                    RoleId = ((int)Domain.Enums.AppRole.Admin).ToString()
                }
            );

            #endregion
        }
    }
}
