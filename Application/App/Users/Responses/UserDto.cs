using AuctionApp.Domain.Models;

namespace Application.App.Users.Responses;
public class UserDto
{
    public int Id { get; set; }

    public required string UserName { get; set; }

    public decimal Balance { get; set; }
}
