using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public DateTime? LastPasswordResetEmailSentAtUtc { get; set; }
}