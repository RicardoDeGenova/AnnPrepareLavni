using AnnPrepareLavni.ApiService.Features.User;
using AnnPrepareLavni.ApiService.Models;

namespace AnnPrepareLavni.ApiService.Data;

public class DatabaseInitialization
{
    public static async Task SeedAdminUserAsync(IUserService userService, IConfiguration configuration)
    {
        var adminUsername = configuration["AdminUser:Username"] ?? "admin";
        var adminPassword = configuration["AdminUser:Password"];

        if (string.IsNullOrEmpty(adminPassword))
        {
            throw new Exception("Admin password not specified. Please set AdminUser:Password in configuration or environment variables.");
        }

        var existingUser = await userService.GetByUsernameAsync(adminUsername);
        if (existingUser != null)
        {
            return;
        }

        var adminUser = new User
        {
            Username = adminUsername,
            PasswordHash = adminPassword,
            Role = UserRole.Administrator,
            Language = Models.Enums.Language.English,
        };

        await userService.CreateAsync(adminUser);

        // Optionally log the creation
        Console.WriteLine($"Admin user '{adminUsername}' has been created.");
    }
}
