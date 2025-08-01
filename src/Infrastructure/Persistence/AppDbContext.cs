using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)); // Dummy data
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<Member>().ToTable("Members");
            builder.Entity<Organization>().ToTable("Organizations");
            builder.Entity<Person>().ToTable("People");

            // 1-to-1 relationships

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

            // 1-to-many relationships

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

            // SQL data types

            builder.Entity<MembershipPlan>()
                .Property(m => m.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<PricingPlan>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            // Constraints

            builder.Entity<MembershipPlan>()
                .HasIndex(m => m.Name)
                .IsUnique();

            // Initial data

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

            // Dummy data

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
        }
    }
}
