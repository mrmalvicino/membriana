using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.Property(e => e.AdmissionDate)
               .IsRequired();

        builder.Property(e => e.OrganizationId)
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired()
               .HasMaxLength(450);

        builder.HasOne(e => e.Organization)
               .WithMany(o => o.Employees)
               .HasForeignKey(e => e.OrganizationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.OrganizationId);
        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => new { e.OrganizationId, e.UserId })
               .IsUnique();
    }
}
