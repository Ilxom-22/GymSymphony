namespace Gymphony.Api.Configurations;

public static partial class HostConfigurations
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder
            .AddDevTools()
            .AddExposers();
        
        return builder;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        app
            .UseDevTools()
            .UseExposers();
        
        return app;
    }
}