using Application.Abstractions;
using Application.App.Bids.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Bids.Commands;

public class CreateBidCommand : IRequest<BidDto>
{
    public required int LotId { get; set; }

    public required int UserId { get; set; }

    public required decimal Amount { get; set; }
}

public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand, BidDto>
{
    private readonly IRepository _repository;

    private readonly CreateBidCommandValidator _validator;

    public CreateBidCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new CreateBidCommandValidator();
    }

    public async Task<BidDto> Handle(CreateBidCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var lot = await _repository.GetById<Lot>(request.LotId)
            ?? throw new ArgumentNullException("Lot cannot be found");

        var auction = lot.Auction;

        if (auction.EndTime <= DateTime.UtcNow)
        {
            throw new ArgumentException("Cannot place bid: Auction Time is out");
        }

        var bid = new Bid()
        {
            LotId = lot.Id,
            UserId = request.UserId,
            Amount = request.Amount,
            CreateTime = DateTime.UtcNow,
            IsWon = false
        };

        await _repository.Add(bid);

        await _repository.SaveChanges();

        var bidDto = BidDto.FromBid(bid);

        return bidDto;
    }
}
