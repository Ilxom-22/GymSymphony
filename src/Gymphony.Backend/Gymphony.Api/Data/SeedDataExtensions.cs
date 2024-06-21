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
                          <body>
                            <p>Hi {{FirstName}},</p>
                            <p>Welcome to the {{CompanyName}} family!</p>
                          
                            <p>We're excited to have you on board and can't wait to support you on your fitness journey.</p>
                          
                            <p>At {{CompanyName}}, we're committed to helping you achieve your goals and enjoy every moment at our gym centers.</p>
                          
                            <p>Need Assistance?</p>
                            <p>If you have any questions or need assistance, feel free to reach out to us. Let's make great things happen together!</p>
                          
                            <p>Best regards,</p>
                            <p>The {{CompanyName}} Team</p>
                          </body>
                          """,
                Type = NotificationType.SystemWelcome  
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Welcome to the {{CompanyName}} Admin Console, {{FirstName}}!",
                Content = """
                          <body>
                            <p>Dear {{FirstName}},</p>
                            <p>Welcome to the {{CompanyName}} Admin Console!</p>
                            <p>We're thrilled to have you join our team!</p>
                          
                            <p>Login Credentials</p>
                            <ul>
                              <li>Login Email Address: {{EmailAddress}}</li>
                              <li>Password: {{Password}}</li>
                            </ul>
                            <p>(This is a temporary password. You'll be prompted to set a new one upon first login.)</p>
                          
                            <p>Getting Started</p>
                            <p>If you have any questions or encounter any difficulties while using the {{CompanyName}} Admin Console, please don't hesitate to reach out to our support team.</p>
                          
                            <p>Welcome aboard!</p>
                          
                            <p>Sincerely,</p>
                            <p>The {{CompanyName}} Team</p>
                          </body>
                          """,
                Type = NotificationType.AdminWelcome
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Action Required: Account Access Blocked on {{CompanyName}} Admin Console",
                Content = """
                          <body>
                            <p>Dear {{FirstName}} {{LastName}},</p>
                            <p>We're contacting you to inform you that your access to the {{CompanyName}} Admin Console has been temporarily blocked.</p>
                          
                            <p>What to Do Next</p>
                            <ul>
                              <li>**Contact Support:** Reply to this email to discuss the reason for the block and initiate the unblocking process.</li>
                              <li>**Security Measures:** (If applicable) Depending on the reason for blockage, you may be required to take additional security measures, such as changing your password or verifying your identity.</li>
                            </ul>
                          
                            <p>We understand this may be inconvenient, and we apologize for any disruption. Our priority is to ensure the security of the Admin Console and the data it contains.</p>
                          
                            <p>Once your account is unblocked, you will receive a confirmation email.</p>
                          
                            <p>Sincerely,</p>
                            <p>The {{CompanyName}} Team</p>
                          </body>
                          """,
                Type = NotificationType.AdminBlockedNotification
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Important Notice: Your Role on the {{CompanyName}} Admin Console Has Been Removed",
                Content = """
                          <body>
                            <p>Dear {{FirstName}} {{LastName}},</p>
                            <p>This email is to inform you that your administrative privileges on the {{CompanyName}} Admin Console have been removed, effective immediately.</p>
                          
                            <p>What to Do Next</p>
                            <p>If you have any questions about this change or believe it may be an error, please don't hesitate to contact us by replying to this email.</p>
                          
                            <p>We appreciate your contributions to the management of the Admin Console.</p>
                          
                            <p>Sincerely,</p>
                            <p>The {{CompanyName}} Team</p>
                          </body>
                          """,
                Type = NotificationType.AdminRemovedNotification
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Your Access to the {{CompanyName}} Admin Console Has Been Restored",
                Content = """
                          <body>
                            <p>Dear {{FirstName}},</p>
                            <p>We're pleased to inform you that your access to the {{CompanyName}} Admin Console has been restored.</p>
                          
                            <p>You can now log in to the console using your existing credentials.</p>
                            <p>**Please note:** To ensure account security, you will be prompted to verify your identity upon your first login.</p>
                          
                            <p>We appreciate your patience and cooperation during this time.</p>
                            <p>If you have any further questions, please don't hesitate to contact our support team.</p>
                          
                            <p>Sincerely,</p>
                            <p>The {{CompanyName}} Team</p>
                          </body>
                          """,
                Type = NotificationType.AdminUnblockedNotification
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Verify Your Email Address for {{CompanyName}}",
                Content = """
                          <body>
                            <p>Dear {{FirstName}},</p>
                            <p>Thank you for signing up for {{CompanyName}}! To complete your account setup, please verify your email address.</p>
                            <p>This helps us ensure the security of your account and allows you to access all the features of {{CompanyName}}.</p>
                            <p>Here's how to verify your email:</p>
                            <p>Click on the following link: <a href="{{VerificationLink}}">Verify Email Address</a></p>
                            <p>We look forward to welcoming you to the {{CompanyName}} community!</p>
                            <p>Sincerely,</p>
                            <p>The {{CompanyName}} Team</p>
                          </body>
                          """,
                Type = NotificationType.EmailVerification
            },
            
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Reset Your Password for {{CompanyName}}",
                Content = """
                          <body>
                            <p>Dear {{FirstName}},</p>
                            <p>We received a request to reset your password for your {{CompanyName}} account.</p>
                          
                            <p>**If you requested this reset:**</p>
                            <p>Click on the following link to create a new password: <a href="{{PasswordResetLink}}">Reset Password</a></p>
                          
                            <p>**If you did not request this reset:**</p>
                            <p>You can safely ignore this email. Your password remains unchanged.</p>
                          
                            <p>Sincerely,</p>
                            <p>The {{CompanyName}} Team</p>
                          </body>
                          """,
                Type = NotificationType.PasswordResetVerification
            }
        };

        await appDbContext.AddRangeAsync(notificationTemplates);
        appDbContext.SaveChanges();
    }
}