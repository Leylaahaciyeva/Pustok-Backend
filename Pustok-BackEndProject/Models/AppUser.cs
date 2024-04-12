using Microsoft.AspNetCore.Identity;

namespace Pustok_BackEndProject.Models;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; } = null!;
}
