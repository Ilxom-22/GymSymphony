using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class CourseScheduleConfiguration : IEntityTypeConfiguration<CourseSchedule>
{
    public void Configure(EntityTypeBuilder<CourseSchedule> builder)
    {
        builder.HasKey(cs => cs.Id);

        builder.HasOne<Course>(cs => cs.Course)
            .WithMany(c => c.Schedules)
            .HasForeignKey(cs => cs.CourseId);

        builder.HasMany(cs => cs.Instructors)
            .WithMany(staff => staff.CourseSchedules);

        builder.HasMany(cs => cs.Enrollments)
            .WithOne(cse => cse.CourseSchedule)
            .HasForeignKey(cse => cse.CourseScheduleId);
        
        builder.HasOne<Admin>(cs => cs.CreatedBy)
            .WithMany()
            .HasForeignKey(cs => cs.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Admin>(cs => cs.ModifiedBy)
            .WithMany()
            .HasForeignKey(cs => cs.ModifiedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Admin>(cs => cs.DeletedBy)
            .WithMany()
            .HasForeignKey(cs => cs.DeletedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasQueryFilter(cs => !cs.IsDeleted);
    }
}