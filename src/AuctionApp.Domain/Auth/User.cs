using AuctionApp.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Domain.Auth;
public class User : IdentityUser<int>, IEntity
{
    public decimal Balance { get; set; } = 0;
}
