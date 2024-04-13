using Microsoft.AspNetCore.Identity;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure;

public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "andrystarik@gmail.com";
        string password = "are you really expecting to see the password here?)";

        string guestEmail = "guest@gmail.com";
        string guestPassword = "Guest_1";

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
            User admin = new User { Email = adminEmail, UserName = adminEmail, Year = 19 };
            IdentityResult result = await userManager.CreateAsync(admin, password);

            User guest = new User { Email = guestEmail, UserName = guestEmail };
            IdentityResult result2 = await userManager.CreateAsync(guest, guestPassword);


            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "super_admin");
            }

            if (result2.Succeeded)
            {
                await userManager.AddToRoleAsync(guest, "user");

            }
        }
    }
}
