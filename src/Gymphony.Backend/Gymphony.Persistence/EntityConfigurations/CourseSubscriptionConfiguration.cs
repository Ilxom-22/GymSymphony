using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class CourseSubscriptionConfiguration : IEntityTypeConfiguration<CourseSubscription>
{
    public void Configure(EntityTypeBuilder<CourseSubscription> builder)
    {
        builder.HasOne<Course>(cs => cs.Course)
            .WithMany(course => course.Subscriptions)
            .HasForeignKey(cs => cs.CourseId);
    }
}