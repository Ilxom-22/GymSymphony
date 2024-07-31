using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class PendingCourseEnrollmentConfiguration : IEntityTypeConfiguration<PendingScheduleEnrollment>
{
    public void Configure(EntityTypeBuilder<PendingScheduleEnrollment> builder)
    {
        builder.HasKey(pce => pce.Id);

        builder.Property(pce => pce.StripeSessionId).IsRequired();

        builder.HasOne(pce => pce.Member)
            .WithMany()
            .HasForeignKey(pce => pce.MemberId);

        builder.HasOne(pce => pce.Course)
            .WithMany()
            .HasForeignKey(pce => pce.CourseId);

        builder.HasOne(pce => pce.CourseSchedule)
            .WithMany(cs => cs.PendingEnrollments)
            .HasForeignKey(pce => pce.CourseScheduleId);
    }
}
