using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class DatabaseServiceExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            await context.Database.MigrateAsync();
            await StoreContextData.SeedAsync(context, userManager);
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during database initialization");
            throw;
        }
    }
}