using Microsoft.AspNetCore.Identity;

namespace MvcCv.Models.Entity;

public sealed class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
