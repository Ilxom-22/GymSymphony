using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Api.Data;

public static class SeedDataExtensions
{
    public static async ValueTask InitializeAsync(this IServiceProvider serviceProvider)
    {
        var appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
        var passwordHasherService = serviceProvider.GetRequiredService<IPasswordHasherService>();
        var webHostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        
        if (!await appDbContext.Admins.AnyAsync())
            await AddDefaultAdminAsync(appDbContext, passwordHasherService, configuration, webHostEnvironment);
    }

    private static async Task AddDefaultAdminAsync(
        AppDbContext appDbContext,
        IPasswordHasherService passwordHasherService,
        IConfiguration configuration,
        IHostEnvironment webHostEnvironment)
    {
        var emailAddress = webHostEnvironment.IsDevelopment()
            ? configuration["AdminEmailAddress"]
            : Environment.GetEnvironmentVariable("AdminEmailAddress");

        var adminPassword = webHostEnvironment.IsDevelopment()
            ? configuration["AdminPassword"]
            : Environment.GetEnvironmentVariable("AdminPassword");

        var adminFirstName = webHostEnvironment.IsDevelopment()
            ? configuration["AdminFirstName"]
            : Environment.GetEnvironmentVariable("AdminFirstName");

        var adminLastName = webHostEnvironment.IsDevelopment()
            ? configuration["AdminLastName"]
            : Environment.GetEnvironmentVariable("AdminLastName");
        
        var admin = new Admin
        {
            Id = Guid.NewGuid(),
            EmailAddress = emailAddress!,
            AuthDataHash = passwordHasherService.HashPassword(adminPassword!),
            Role = Role.Admin,
            Status = AccountStatus.Active,
            CreatedTime = DateTimeOffset.UtcNow,
            FirstName = adminFirstName!,
            LastName = adminLastName!
        };

        await appDbContext.Admins.AddAsync(admin);
        appDbContext.SaveChanges();
    }
}