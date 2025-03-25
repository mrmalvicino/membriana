using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Member>().ToTable("Members");
            modelBuilder.Entity<Organization>().ToTable("Organizations");
            modelBuilder.Entity<Person>().ToTable("People");

            // 1-to-1 relationships

            modelBuilder.Entity<Organization>()
                .HasOne(o => o.LogoImage)
                .WithOne()
                .HasForeignKey<Organization>(o => o.LogoImageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.ProfileImage)
                .WithOne()
                .HasForeignKey<Person>(p => p.ProfileImageId)
                .OnDelete(DeleteBehavior.NoAction);

            // 1-to-many relationships

            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Employees)
                .WithOne(e => e.Organization)
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Members)
                .WithOne(m => m.Organization)
                .HasForeignKey(m => m.OrganizationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Organization>()
                .HasMany(o => o.MembershipPlans)
                .WithOne(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Member>()
                .HasMany(p => p.Payments)
                .WithOne(m => m.Member)
                .HasForeignKey(m => m.MemberId)
                .OnDelete(DeleteBehavior.NoAction);

            // SQL data types

            modelBuilder.Entity<MembershipPlan>()
                .Property(m => m.Fee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PricingPlan>()
                .Property(p => p.Fee)
                .HasColumnType("decimal(18,2)");
        }
    }
}
