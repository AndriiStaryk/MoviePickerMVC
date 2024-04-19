using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Controllers;

[Authorize(Roles = Accessibility.Roles)]

public class RolesController : Controller
{
    RoleManager<IdentityRole> _roleManager;
    UserManager<User> _userManager;
    public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    public IActionResult Index() => View(_roleManager.Roles.ToList());
    public IActionResult UserList() => View(_userManager.Users.ToList());

    public async Task<IActionResult> Edit(string userId)
    { 
        User user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            ChangeRoleViewModel model = new ChangeRoleViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles
            };
            return View(model);
        }

        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Edit(string userId, List<string> roles)
    {
        // отримуємо користувача
        User user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // список ролей користувача
            var userRoles = await _userManager.GetRolesAsync(user);

            bool isSuperAdmin = userRoles.Contains("super_admin");

            if (isSuperAdmin)
            {
                roles.Add("super_admin");
            }

          
            // Remove any roles that the user currently has but were not submitted in the form data
            var rolesToRemove = userRoles.Except(roles);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            // Add any roles that were submitted in the form data but the user does not currently have
            var rolesToAdd = roles.Except(userRoles);
            await _userManager.AddToRolesAsync(user, rolesToAdd);



            return RedirectToAction("UserList");
        }

        return NotFound();
    }

}
