using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.StripeSubscriptionId).IsRequired();
        
        builder.HasDiscriminator(s => s.Type)
            .HasValue<MembershipPlanSubscription>(SubscriptionType.MembershipSubscription)
            .HasValue<CourseSubscription>(SubscriptionType.CourseSubscription);

        builder.HasOne<Member>(s => s.Member)
            .WithMany(member => member.Subscriptions)
            .HasForeignKey(s => s.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SubscriptionPeriod>(s => s.LastSubscriptionPeriod)
            .WithOne()
            .HasForeignKey<Subscription>(s => s.LastSubscriptionPeriodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}