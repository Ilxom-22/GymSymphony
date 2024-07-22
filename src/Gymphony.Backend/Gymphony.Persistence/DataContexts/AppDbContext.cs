using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.DataContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region Identity
    public DbSet<User> Users => Set<User>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<AccessToken> AccessTokens => Set<AccessToken>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    #endregion

    #region Notifications
    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();
    public DbSet<NotificationHistory> NotificationHistories => Set<NotificationHistory>();
    public DbSet<VerificationToken> VerificationTokens => Set<VerificationToken>();

    #endregion

    #region Products
    public DbSet<Product> Products => Set<Product>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<Course> Courses => Set<Course>();

    #endregion

    #region Subscription & Payments
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<SubscriptionPeriod> SubscriptionPeriods => Set<SubscriptionPeriod>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<MembershipPlanSubscription> MembershipPlanSubscriptions => Set<MembershipPlanSubscription>();
    public DbSet<CourseSubscription> CourseSubscriptions => Set<CourseSubscription>();

    #endregion

    #region Course Schedules
    public DbSet<CourseSchedule> CourseSchedules => Set<CourseSchedule>();
    public DbSet<CourseScheduleEnrollment> CourseScheduleEnrollments => Set<CourseScheduleEnrollment>();
    public DbSet<PendingScheduleEnrollment> PendingScheduleEnrollments => Set<PendingScheduleEnrollment>();

    #endregion

    #region Files
    public DbSet<StorageFile> StorageFiles => Set<StorageFile>();
    public DbSet<CourseImage> CourseImages => Set<CourseImage>();

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}