using Microsoft.AspNetCore.Identity;
using MyDiplomDelivery.Models;

namespace MyDiplomDelivery
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";
            
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await roleManager.FindByNameAsync("DeliveryMan") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("DeliveryMan"));
            }

            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (await roleManager.FindByNameAsync("Manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}