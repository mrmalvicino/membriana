using Contracts.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.Property(m => m.AdmissionDate)
               .IsRequired();

        builder.Property(m => m.MemberStatus)
               .HasConversion<int>()
               .HasDefaultValue(MemberStatus.Active)
               .IsRequired();

        builder.HasOne(m => m.Organization)
               .WithMany(o => o.Members)
               .HasForeignKey(m => m.OrganizationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.MembershipPlan)
               .WithMany(p => p.Members)
               .HasForeignKey(m => m.MembershipPlanId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Payments)
               .WithOne(p => p.Member)
               .HasForeignKey(p => p.MemberId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => new { m.OrganizationId, m.MemberStatus });
    }
}
