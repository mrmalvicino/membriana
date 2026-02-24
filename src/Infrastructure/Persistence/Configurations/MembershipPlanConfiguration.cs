using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
    {
        public void Configure(EntityTypeBuilder<MembershipPlan> builder)
        {
            builder.ToTable("MembershipPlans");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(m => m.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(m => m.Organization)
                   .WithMany(o => o.MembershipPlans)
                   .HasForeignKey(m => m.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => new { m.OrganizationId, m.Name })
                   .IsUnique();
        }
    }
}
