using Microsoft.AspNetCore.Identity;

namespace Domain.Auth;
public class User : IdentityUser<int>
{
    public decimal Balance { get; set; } = 0;
}
