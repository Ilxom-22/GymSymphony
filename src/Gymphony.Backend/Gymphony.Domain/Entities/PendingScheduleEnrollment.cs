using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class PendingScheduleEnrollment : Entity
{
    public Guid CourseId { get; set; }

    public Guid MemberId { get; set; }

    public Guid CourseScheduleId { get; set; }

    public string StripeSessionId { get; set; } = default!;

    public Member? Member { get; set; }

    public Course? Course { get; set; }

    public CourseSchedule? CourseSchedule { get; set; }
}
