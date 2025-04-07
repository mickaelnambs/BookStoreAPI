using System.Text.Json;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public static class StoreContextData
{
    public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any(x => x.UserName == "admin@test.com"))
        {
            var user = new AppUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com"
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        if (!context.Books.Any())
        {
            var booksData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/books.json");

            var books = JsonSerializer.Deserialize<List<Book>>(booksData);

            if (books == null) return;

            context.Books.AddRange(books);

            await context.SaveChangesAsync();
        }

        if (!context.DeliveryMethods.Any())
        {
            var dmData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

            var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

            if (methods == null) return;

            context.DeliveryMethods.AddRange(methods);

            await context.SaveChangesAsync();
        }
    }
}
