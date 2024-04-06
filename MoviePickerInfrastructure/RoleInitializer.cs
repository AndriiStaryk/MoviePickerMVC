using Microsoft.AspNetCore.Identity;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure;

public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "andrystarik@gmail.com";
        string password = "are you really expecting to see the password here?)";
        if (await roleManager.FindByNameAsync("admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
        }
        if (await roleManager.FindByNameAsync("user") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("user"));
        }
        if (await roleManager.FindByNameAsync("super_admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("super_admin"));
        }
        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            User admin = new User { Email = adminEmail, UserName = adminEmail };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "super_admin");
            }
        }
    }
}
