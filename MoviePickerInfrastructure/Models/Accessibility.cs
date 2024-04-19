using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MoviePickerDomain.Model;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MoviePickerInfrastructure.Models;
public class Accessibility
{
    public const string Roles = "admin, super_admin";

    public static bool IsUserAuthenticated(ClaimsPrincipal user)
    {
        return user.Identity.IsAuthenticated;
    }

    public static bool AllExceptUser(ClaimsPrincipal user)
    {
        if (!IsUserAuthenticated(user))
        {
            return false;
        }
        else
        {
            var userRoles = user.Claims
                             .Where(c => c.Type == ClaimTypes.Role)
                             .Select(c => c.Value);

            return !(userRoles.Count() == 1 && userRoles.Contains("user"));
        }
    
    }
}

