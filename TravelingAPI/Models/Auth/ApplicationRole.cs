using Microsoft.AspNetCore.Identity;

namespace TravelingAPI.Models.Auth;

public class ApplicationRole : IdentityRole
{
    public ICollection<ApplicationUserRole> UserRoles { get; set; }
}