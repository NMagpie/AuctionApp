using Application.Abstractions;
using Application.App.Auctions.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.App.Auctions.Commands;

public class DeleteAuctionCommand : IRequest<AuctionDto>
{
    public int Id { get; set; }
}

public class DeleteAuctionCommandHandler : IRequestHandler<DeleteAuctionCommand, AuctionDto>
{
    private readonly IRepository _repository;

    private readonly ILogger<DeleteAuctionCommandHandler> _logger;

    private readonly IMapper _mapper;

    public DeleteAuctionCommandHandler(IRepository repository, ILogger<DeleteAuctionCommandHandler> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<AuctionDto> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<Auction>(request.Id)
            ?? throw new ArgumentNullException("Auction cannot be found");

        if (auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new ArgumentException("Cannot delete auction 5 minutes before its start");
        }

        auction = await _repository.Remove<Auction>(request.Id);

        await _repository.SaveChanges();

        var auctionDto = _mapper.Map<Auction, AuctionDto>(auction);

        _logger.LogInformation($"[{DateTime.UtcNow}]-[{this.GetType().Name}] was executed successfully!");

        return auctionDto;
    }
}
