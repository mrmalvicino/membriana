using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MemberStatusEventConfiguration : IEntityTypeConfiguration<MemberStatusEvent>
    {
        public void Configure(EntityTypeBuilder<MemberStatusEvent> builder)
        {
            builder.ToTable("MemberStatusEvents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.OrganizationId)
                   .IsRequired();

            builder.Property(x => x.MemberId)
                   .IsRequired();

            builder.Property(x => x.PreviousStatus)
                   .HasConversion<int?>()
                   .IsRequired(false);

            builder.Property(x => x.NewStatus)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(x => x.ChangedAtDateTime)
                   .IsRequired();

            builder.Property(x => x.ChangedAtDateTime)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.ChangedByUserId)
                   .IsRequired()
                   .HasMaxLength(450);

            builder.Property(x => x.Details)
                   .HasMaxLength(1000)
                    .IsRequired(false);

            builder.HasOne(x => x.Organization)
                   .WithMany()
                   .HasForeignKey(x => x.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Member)
                   .WithMany()
                   .HasForeignKey(x => x.MemberId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ChangedByUser)
                   .WithMany()
                   .HasForeignKey(x => x.ChangedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.OrganizationId, x.ChangedAtDateTime });
            builder.HasIndex(x => new { x.OrganizationId, x.NewStatus, x.ChangedAtDateTime });
            builder.HasIndex(x => new { x.OrganizationId, x.MemberId, x.ChangedAtDateTime });
        }
    }
}
