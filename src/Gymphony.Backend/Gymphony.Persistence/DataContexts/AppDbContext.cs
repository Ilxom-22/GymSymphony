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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}