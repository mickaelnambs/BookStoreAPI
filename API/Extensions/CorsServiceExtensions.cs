namespace API.Extensions;

public static class CorsServiceExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors();
        return services;
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        app.UseCors(x => x.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:4200", "https://localhost:4200"));

        return app;
    }
}