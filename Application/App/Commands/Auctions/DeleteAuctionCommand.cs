using MediatR;

namespace Application.App.Commands.Auctions;
public class DeleteAuctionCommand : IRequest
{
    public int Id { get; set; }
}
