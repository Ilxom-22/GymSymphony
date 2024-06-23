using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.DataContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Admin> Admins => Set<Admin>();

    public DbSet<Member> Members => Set<Member>();

    public DbSet<AccessToken> AccessTokens => Set<AccessToken>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();

    public DbSet<NotificationHistory> NotificationHistories => Set<NotificationHistory>();

    public DbSet<VerificationToken> VerificationTokens => Set<VerificationToken>();

    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}