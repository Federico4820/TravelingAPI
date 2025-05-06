using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TravelingAPI.Models.Auth;

public class ApplicationUserRole : IdentityUserRole<string>
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid RoleId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public ApplicationUser ApplicationUser { get; set; }
    
    [ForeignKey(nameof(RoleId))]
    public ApplicationRole ApplicationRole { get; set; }
}