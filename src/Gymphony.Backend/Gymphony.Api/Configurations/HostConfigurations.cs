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
            .AddProductsInfrastructure()
            .AddValidators()
            .AddMappers()
            .AddPaymentInfrastructure()
            .AddSubscriptionsInfrastructure()
            .AddFilesInfrastructure();
        
        return builder;
    }

    public static async ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
    {
        app
            .UseCors()
            .UseDevTools()
            .UseExposers()
            .UseAuthentication()
            .UseAuthorization();

        await app.MigrateDatabaseSchemaAsync();
        await app.SeedDataAsync();

        return app;
    }
}