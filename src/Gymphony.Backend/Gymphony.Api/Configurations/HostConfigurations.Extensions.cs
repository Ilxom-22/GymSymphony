using System.Reflection;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Domain.Brokers;
using Gymphony.Infrastructure.Common.EventBus.Brokers;
using Gymphony.Infrastructure.Identity.Brokers;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Extensions;
using Gymphony.Persistence.Interceptors;
using Gymphony.Persistence.Repositories;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        builder.Services.AddControllers();

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

    private static WebApplicationBuilder AddUsersInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IAdminRepository, AdminRepository>()
            .AddScoped<IMemberRepository, MemberRepository>();
        
        return builder;
    }
    
    private static async ValueTask<WebApplication> MigrateDatabaseSchemaAsync(this WebApplication app)
    {
        var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        await serviceScopeFactory.MigrateAsync<AppDbContext>();
        
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