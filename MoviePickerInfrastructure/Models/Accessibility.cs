using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models;
public class Accessibility
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    public Accessibility(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
    public static bool AllExceptUser(ClaimsPrincipal user)
    {
        return !user.IsInRole("user");
    }
}
