using Application.Abstractions;
using Application.App.Auctions.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Auctions.Commands;

public class UpdateAuctionCommand : IRequest<AuctionDto>
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }
}

public class UpdateAuctionCommandHandler : IRequestHandler<UpdateAuctionCommand, AuctionDto>
{
    private readonly IRepository _repository;

    private readonly UpdateAuctionCommandValidator _validator;

    public UpdateAuctionCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new UpdateAuctionCommandValidator();
    }

    public async Task<AuctionDto> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var auction = await _repository.GetById<Auction>(request.Id)
            ?? throw new ArgumentNullException("Auction cannot be found");

        if (auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new ArgumentException("Cannot edit auction 5 minutes before its start");
        }

        auction.Title = request.Title ?? auction.Title;
        auction.StartTime = request.StartTime ?? auction.StartTime;
        auction.EndTime = request.EndTime ?? auction.EndTime;

        await _repository.SaveChanges();

        var auctionDto = AuctionDto.FromAuction(auction);

        return auctionDto;
    }
}
