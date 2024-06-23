using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
{
    public void Configure(EntityTypeBuilder<MembershipPlan> builder)
    {
        builder.HasKey(plan => plan.Id);

        builder.HasIndex(plan => plan.Name).IsUnique();
        builder.Property(plan => plan.Name).HasMaxLength(256);
        builder.Property(plan => plan.Description).IsRequired().HasMaxLength(2048);
        builder.Property(plan => plan.DurationInDays).IsRequired();
        builder.Property(plan => plan.Price).IsRequired().HasColumnType("numeric(18,2)");

        builder.HasOne<Admin>(plan => plan.CreatedBy)
            .WithMany()
            .HasForeignKey(plan => plan.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Admin>(plan => plan.ModifiedBy)
            .WithMany()
            .HasForeignKey(plan => plan.ModifiedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}