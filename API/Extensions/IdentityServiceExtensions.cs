using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddIdentityApiEndpoints<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<StoreContext>();

        return services;
    }

    public static WebApplication MapIdentityEndpoints(this WebApplication app)
    {
        app.MapGroup("api").MapIdentityApi<AppUser>();
        return app;
    }
}