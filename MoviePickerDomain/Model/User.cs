using Microsoft.AspNetCore.Identity;

namespace MoviePickerDomain.Model;

public class User : IdentityUser
{
    public int Year { get; set; }

    public byte[]? Avatar { get; set; }
}
