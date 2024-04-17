using AuctionApp.Domain.Models;

namespace Application.App.Responses;
public class UserDto
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public static UserDto FromUser(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
        };
    }
}
