using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.OrganizationId)
                   .IsRequired();

            builder.HasOne(u => u.Organization)
                   .WithMany()
                   .HasForeignKey(u => u.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Employee)
                   .WithOne(e => e.User)
                   .HasForeignKey<Employee>(e => e.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Member)
                   .WithOne(m => m.User)
                   .HasForeignKey<Member>(m => m.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(u => u.OrganizationId);
        }
    }
}
