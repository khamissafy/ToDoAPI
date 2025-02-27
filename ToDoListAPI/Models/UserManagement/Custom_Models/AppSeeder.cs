using Microsoft.AspNetCore.Identity;
using ToDoListAPI.Models.UserManagement.DB_Models;

public class AppSeeder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public AppSeeder(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task SeedAdminUserAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if the admin user exists, and create it if not
            var adminEmail = _configuration["AdminSettings:Email"];
            var adminPassword = _configuration["AdminSettings:Password"];
            if (adminEmail != null)
            {
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        FullName = adminEmail,
                        isActive = true,
                    };
                    if (adminPassword != null)
                    {
                       await userManager.CreateAsync(adminUser, adminPassword);
                    }
                    else
                    {
                        Console.WriteLine("No Seeder done");
                    }
                }
            }
        }
    }
    
    
    
}
