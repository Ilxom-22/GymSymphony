using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class CourseScheduleEnrollmentConfiguration : IEntityTypeConfiguration<CourseScheduleEnrollment>
{
    public void Configure(EntityTypeBuilder<CourseScheduleEnrollment> builder)
    {
        builder.HasKey(cse => cse.Id);

        builder.Property(cse => cse.EnrollmentDate).IsRequired();

        builder.HasOne<Member>(cse => cse.Member)
            .WithMany()
            .HasForeignKey(cse => cse.MemberId);

        builder.HasOne<CourseSubscription>(cse => cse.CourseSubscription)
            .WithMany()
            .HasForeignKey(cse => cse.CourseSubscriptionId);
    }
}