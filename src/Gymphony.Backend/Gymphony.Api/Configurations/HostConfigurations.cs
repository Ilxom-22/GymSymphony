namespace Gymphony.Api.Configurations;

public static partial class HostConfigurations
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder
            .AddCors()
            .AddDevTools()
            .AddExposers()
            .AddPersistence()
            .AddMediator()
            .AddEventBus()
            .AddRequestContextTools()
            .AddJwtAuthentication()
            .AddIdentityInfrastructure()
            .AddUsersInfrastructure()
            .AddNotificationsInfrastructure()
            .AddMembershipPlansInfrastructure()
            .AddValidators()
            .AddMappers()
            .AddPaymentInfrastructure();
        
        return builder;
    }

    public static async ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
    {
        app
            .UseCors()
            .UseDevTools()
            .UseExposers();

        await app.MigrateDatabaseSchemaAsync();
        await app.SeedDataAsync();

        return app;
    }
}