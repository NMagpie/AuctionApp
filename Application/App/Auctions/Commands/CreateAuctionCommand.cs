using Application.Abstractions;
using Application.App.Auctions.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Auctions.Commands;

public class CreateAuctionCommand : IRequest<AuctionDto>
{
    public required string Title { get; set; }

    public int CreatorId { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset EndTime { get; set; }

    public List<Lot> Lots { get; set; } = [];
}

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, AuctionDto>
{
    private readonly IRepository _repository;

    private readonly CreateAuctionCommandValidator _validator;

    public CreateAuctionCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new CreateAuctionCommandValidator();
    }

    public async Task<AuctionDto> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = await _repository.GetById<User>(request.CreatorId)
            ?? throw new ArgumentNullException("User cannot be found");

        var auction = new Auction()
        {
            Title = request.Title,
            CreatorId = request.CreatorId,
            Creator = user,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Lots = request.Lots
        };

        await _repository.Add(auction);

        await _repository.SaveChanges();

        var auctionDto = AuctionDto.FromAuction(auction);

        return auctionDto;
    }
}
