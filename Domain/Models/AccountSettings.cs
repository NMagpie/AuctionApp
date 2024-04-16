using AuctionApp.Domain.Models;
using EntityFramework.Domain.Abstractions;

namespace Domain.Models;
public class AccountSettings : Entity
{
    public int UserId { get; set; }

    public User User { get; set; }

    public string? BillingInfo { get; set; }

    public string? TwoFactorAuthentication { get; set; }
}
