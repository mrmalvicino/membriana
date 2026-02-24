using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organizations");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Active).IsRequired();

            builder.Property(o => o.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(o => o.Email)
                   .IsRequired()
                   .HasMaxLength(254);

            builder.Property(o => o.Phone)
                   .HasMaxLength(50);

            builder.HasOne(o => o.LogoImage)
                   .WithOne()
                   .HasForeignKey<Organization>(o => o.LogoImageId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.Employees)
                   .WithOne(e => e.Organization)
                   .HasForeignKey(e => e.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.Members)
                   .WithOne(m => m.Organization)
                   .HasForeignKey(m => m.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.MembershipPlans)
                   .WithOne(p => p.Organization)
                   .HasForeignKey(p => p.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.PricingPlan)
                   .WithMany(p => p.Organizations)
                   .HasForeignKey(o => o.PricingPlanId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
