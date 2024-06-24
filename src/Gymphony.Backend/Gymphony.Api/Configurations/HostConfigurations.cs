namespace Gymphony.Api.Configurations;

public static partial class HostConfigurations
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder
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
            .UseDevTools()
            .UseExposers();

        await app.MigrateDatabaseSchemaAsync();
        await app.SeedDataAsync();

        return app;
    }
}