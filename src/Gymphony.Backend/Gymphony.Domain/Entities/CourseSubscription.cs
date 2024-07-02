namespace Gymphony.Domain.Entities;

public class CourseSubscription : Subscription
{
    public Guid CourseId { get; set; }

    public Course? Course { get; set; }
}