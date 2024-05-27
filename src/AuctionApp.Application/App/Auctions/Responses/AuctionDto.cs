using Application.App.Lots.Responses;
using Application.App.Users.Responses;

namespace Application.App.Auctions.Responses;
public class AuctionDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public UserDto? Creator { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public List<LotDto> Lots { get; set; } = [];
}