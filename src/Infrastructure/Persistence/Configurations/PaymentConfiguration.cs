using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Active).IsRequired();

        builder.Property(p => p.DateTime).IsRequired();

        builder.Property(p => p.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(p => p.Member)
               .WithMany(m => m.Payments)
               .HasForeignKey(p => p.MemberId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Organization)
               .WithMany()
               .HasForeignKey(p => p.OrganizationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.OrganizationId, p.DateTime });
        builder.HasIndex(p => new { p.OrganizationId, p.Active, p.DateTime });
    }
}
