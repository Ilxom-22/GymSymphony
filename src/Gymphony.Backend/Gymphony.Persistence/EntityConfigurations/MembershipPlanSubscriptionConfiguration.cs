using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class MembershipPlanSubscriptionConfiguration : IEntityTypeConfiguration<MembershipPlanSubscription>
{
    public void Configure(EntityTypeBuilder<MembershipPlanSubscription> builder)
    {
        builder.HasOne<MembershipPlan>(mps => mps.MembershipPlan)
            .WithMany(mp => mp.Subscriptions)
            .HasForeignKey(mps => mps.MembershipPlanId);
    }
}