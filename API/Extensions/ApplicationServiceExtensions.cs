using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<StoreContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var connString = config.GetConnectionString("Redis")
                ?? throw new Exception("Cannot get redis connection string");
            var configuration = ConfigurationOptions.Parse(connString, true);
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookImportService, BookImportService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddSingleton<ICartService, CartService>();
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();

        return services;
    }
}