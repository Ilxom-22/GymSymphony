using System.Reflection;
using System.Text;
using FluentValidation;
using Gymphony.Api.Data;
using Gymphony.Api.Filters;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Models.Settings;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Brokers;
using Gymphony.Infrastructure.Common.EventBus.Brokers;
using Gymphony.Infrastructure.Common.Identity.Brokers;
using Gymphony.Infrastructure.Common.Identity.Services;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Extensions;
using Gymphony.Persistence.Interceptors;
using Gymphony.Persistence.Repositories;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Gymphony.Api.Configurations;

public static partial class HostConfigurations
{
    private static readonly ICollection<Assembly> Assemblies = Assembly
        .GetExecutingAssembly()
        .GetReferencedAssemblies()
        .Select(Assembly.Load)
        .Append(Assembly.GetExecutingAssembly())
        .ToList();
    
    private static WebApplicationBuilder AddDevTools(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }
    
    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers(configs => configs.Filters
            .Add<AccessTokenValidationFilter>());

        return builder;
    }

    private static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    { 
        builder.Services
            .AddScoped<UpdatePrimaryKeyInterceptor>()
            .AddScoped<UpdateAuditableInterceptor>()
            .AddScoped<UpdateSoftDeletionInterceptor>();
        
        var dbConnectionString = builder.Environment.IsDevelopment()
            ? builder.Configuration.GetConnectionString("DbConnectionString")
            : Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_DbConnectionString");

        builder.Services.AddDbContext<AppDbContext>((provider, options) =>
        {
            options
                .UseNpgsql(dbConnectionString)
                .AddInterceptors(
                    provider.GetRequiredService<UpdatePrimaryKeyInterceptor>(),
                    provider.GetRequiredService<UpdateAuditableInterceptor>(),
                    provider.GetRequiredService<UpdateSoftDeletionInterceptor>());
        });
        
        return builder;
    }

    private static WebApplicationBuilder AddMediator(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(conf 
            => {conf.RegisterServicesFromAssemblies(Assemblies.ToArray());});
        
        return builder;
    }

    private static WebApplicationBuilder AddEventBus(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventBusBroker, EventBusBroker>();

        return builder;
    }

    private static WebApplicationBuilder AddRequestContextTools(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddScoped<IRequestContextProvider, RequestContextProvider>();
        
        return builder;
    }
    
    private static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

        builder.Services.Configure<RefreshTokenSettings>(
            builder.Configuration.GetSection(nameof(RefreshTokenSettings)));

        var jwtSecretKey = (builder.Environment.IsDevelopment()
            ? builder.Configuration["JwtSecretKey"]
            : Environment.GetEnvironmentVariable("JwtSecretKey"))
                ?? throw new InvalidOperationException("JwtSecretKey is not configured!");

        builder.Services.Configure<JwtSecretKey>(
            options => options.SecretKey = jwtSecretKey);
        
        var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ??
                          throw new InvalidOperationException("JwtSettings is not configured.");

        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = jwtSettings.ValidateIssuer,
                        ValidIssuer = jwtSettings.ValidIssuer,
                        ValidAudience = jwtSettings.ValidAudience,
                        ValidateAudience = jwtSettings.ValidateAudience,
                        ValidateLifetime = jwtSettings.ValidateLifetime,
                        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
                    };
                }
            );
        
        builder.Services
            .AddTransient<IAccessTokenGeneratorService, AccessTokenGeneratorService>()
            .AddTransient<IRefreshTokenGeneratorService, RefreshTokenGeneratorService>();

        builder.Services
            .AddScoped<IAccessTokenRepository, AccessTokenRepository>()
            .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        
        return builder;
    }

    private static WebApplicationBuilder AddUsersInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IAdminRepository, AdminRepository>()
            .AddScoped<IMemberRepository, MemberRepository>();
        
        return builder;
    }
    
    private static WebApplicationBuilder AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<IPasswordHasherService, PasswordHasherService>();
        
        return builder;
    }

    private static WebApplicationBuilder AddValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblies(Assemblies);
        
        return builder;
    }

    private static WebApplicationBuilder AddMappers(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(Assemblies);

        return builder;
    }
    
    private static async ValueTask<WebApplication> MigrateDatabaseSchemaAsync(this WebApplication app)
    {
        var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        await serviceScopeFactory.MigrateAsync<AppDbContext>();
        
        return app;
    }
    
    private static async ValueTask<WebApplication> SeedDataAsync(this WebApplication app)
    {
        var serviceScope = app.Services.CreateScope();
        await serviceScope.ServiceProvider.InitializeAsync();

        return app;
    }
    
    private static WebApplication UseDevTools(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
    
    private static WebApplication UseExposers(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }
}