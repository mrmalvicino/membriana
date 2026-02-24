using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Active).IsRequired();

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.Email)
                   .IsRequired()
                   .HasMaxLength(254);

            builder.Property(p => p.Phone)
                   .HasMaxLength(50);

            builder.Property(p => p.Dni)
                   .HasMaxLength(50);

            builder.Property(p => p.BirthDate)
                   .IsRequired();

            builder.HasOne(p => p.ProfileImage)
                   .WithOne()
                   .HasForeignKey<Person>(p => p.ProfileImageId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
