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

        if (!await appDbContext.NotificationTemplates.AnyAsync())
            await SeedNotificationTemplatesAsync(appDbContext);
    }

    private static async ValueTask AddDefaultAdminAsync(
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
    
    private static async ValueTask SeedNotificationTemplatesAsync(AppDbContext appDbContext)
    {
        var notificationTemplates = new List<NotificationTemplate>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Welcome to {{CompanyName}}",
                Content = """
                          Hi {{FirstName}},
                          
                          Welcome to the {{CompanyName}} family!
                          
                          We're excited to have you on board and can't wait to support you on your fitness journey. At {{CompanyName}}, we're committed to helping you achieve your goals and enjoy every moment at our gym centers.
                          
                          If you have any questions or need assistance, feel free to reach out to us. Let's make great things happen together!
                          
                          Best regards,
                          The {{CompanyName}} Team
                          """,
                Type = NotificationType.SystemWelcome  
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Welcome to the {{CompanyName}} Admin Console, {{FirstName}}!",
                Content = """
                          Dear {{FirstName}},
                          
                          Welcome to the {{CompanyName}} Admin Console! We're thrilled to have you join our team!
                          
                          Login Credentials:
                          Login Email Address: {{EmailAddress}}
                          Password: {{Password}}
                          (This is a temporary password. You'll be prompted to set a new one upon first login.)
                          
                          If you have any questions or encounter any difficulties while using the {{CompanyName}} Admin Console, please don't hesitate to reach out to our support team.
                          
                          Welcome aboard!
                          
                          Sincerely,
                          
                          The {{CompanyName}} Team
                          """,
                Type = NotificationType.AdminWelcome
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Action Required: Account Access Blocked on {{CompanyName}} Admin Console",
                Content = """
                          Dear {{FirstName}} {{LastName}},
                          
                          We're contacting you to inform you that your access to the {{CompanyName}} Admin Console has been temporarily blocked.
                          
                          What to Do Next:
                          
                          To regain access to the Admin Console, please follow these steps:
                          
                          Contact Support: Reply to this email to discuss the reason for the block and initiate the unblocking process.
                          Security Measures: (If applicable) Depending on the reason for blockage, you may be required to take additional security measures, such as changing your password or verifying your identity.
                          We understand this may be inconvenient, and we apologize for any disruption. Our priority is to ensure the security of the Admin Console and the data it contains.
                          
                          Once your account is unblocked, you will receive a confirmation email.
                          
                          Sincerely,
                          
                          The {{CompanyName}} Team
                          """,
                Type = NotificationType.AdminBlockedNotification
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Important Notice: Your Role on the {{CompanyName}} Admin Console Has Been Removed",
                Content = """
                          Dear {{FirstName}} {{LastName}},
                          
                          This email is to inform you that your administrative privileges on the {{CompanyName}} Admin Console have been removed, effective immediately.
                          
                          What to Do Next:
                          
                          If you have any questions about this change or believe it may be an error, please don't hesitate to contact us by replying to this email.
                          
                          We appreciate your contributions to the management of the Admin Console.
                          
                          Sincerely,
                          
                          The {{CompanyName}} Team
                          """,
                Type = NotificationType.AdminRemovedNotification
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Your Access to the {{CompanyName}} Admin Console Has Been Restored",
                Content = """
                          Dear {{FirstName}},
                          
                          We're pleased to inform you that your access to the {{CompanyName}} Admin Console has been restored.
                          
                          You can now log in to the console using your existing credentials. Please take into account that to ensure account security you will be asked to verify your identity on your first log in.
                          
                          We appreciate your patience and cooperation during this time.
                          
                          If you have any further questions, please don't hesitate to contact our support team.
                          
                          Sincerely,
                          
                          The {{CompanyName}} Team
                          """,
                Type = NotificationType.AdminUnblockedNotification
            }
        };

        await appDbContext.AddRangeAsync(notificationTemplates);
        appDbContext.SaveChanges();
    }
}