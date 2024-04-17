using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Bids;
public class DeleteBidCommand : IRequest<BidDto>
{
    public int Id { get; set; }
}
