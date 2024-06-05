using System.Reflection.Metadata;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Api.Configurations;

public static partial class HostConfigurations
{
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
        var dbConnectionString = builder.Environment.IsDevelopment()
            ? builder.Configuration.GetConnectionString("DbConnectionString")
            : Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_DbConnectionString");

        builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(dbConnectionString));
        
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