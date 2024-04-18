using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Auctions;
public class DeleteAuctionCommand : IRequest<AuctionDto>
{
    public int Id { get; set; }
}
