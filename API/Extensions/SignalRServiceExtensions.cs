using API.SignalR;

namespace API.Extensions;

public static class SignalRServiceExtensions
{
    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }

    public static WebApplication MapSignalRHubs(this WebApplication app)
    {
        app.MapHub<NotificationHub>("/hub/notifications");
        return app;
    }
}