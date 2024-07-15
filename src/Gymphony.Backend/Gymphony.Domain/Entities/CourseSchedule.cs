using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class CourseSchedule : AuditableSoftDeletedEntity, ICreationAuditableEntity, IModificationAuditableEntity, IDeletionAuditableEntity
{
    public Guid CourseId { get; set; }

    public DayOfWeek Day { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public Course? Course { get; set; }

    public virtual ICollection<Staff>? Instructors { get; set; }

    public virtual List<CourseScheduleEnrollment>? Enrollments { get; set; }
    
    public Guid? CreatedByUserId { get; set; }
    
    public Guid? ModifiedByUserId { get; set; }
    
    public Guid? DeletedByUserId { get; set; }

    public Admin? CreatedBy { get; set; }

    public Admin? ModifiedBy { get; set; }

    public Admin? DeletedBy { get; set; }
}