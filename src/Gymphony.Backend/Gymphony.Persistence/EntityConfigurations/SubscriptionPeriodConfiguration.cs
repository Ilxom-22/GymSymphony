using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class SubscriptionPeriodConfiguration : IEntityTypeConfiguration<SubscriptionPeriod>
{
    public void Configure(EntityTypeBuilder<SubscriptionPeriod> builder)
    {
        builder.HasKey(sp => sp.Id);

        builder.HasOne<Payment>(sp => sp.Payment)
            .WithOne(payment => payment.SubscriptionPeriod)
            .HasForeignKey<SubscriptionPeriod>(sp => sp.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Subscription>(sp => sp.Subscription)
            .WithMany(s => s.SubscriptionPeriods)
            .HasForeignKey(sp => sp.SubscriptionId);
    }
}