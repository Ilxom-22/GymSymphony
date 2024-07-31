using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class CourseScheduleEnrollment : SoftDeletedEntity
{
    public Guid CourseScheduleId { get; set; }

    public Guid MemberId { get; set; }

    public Guid CourseSubscriptionId { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public CourseSubscription? CourseSubscription { get; set; }

    public CourseSchedule? CourseSchedule { get; set; }

    public Member? Member { get; set; }
}