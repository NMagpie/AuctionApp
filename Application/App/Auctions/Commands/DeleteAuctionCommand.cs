using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.App.Auctions.Commands;

public class DeleteAuctionCommand : IRequest
{
    public int Id { get; set; }

    public int CreatorId { get; set; }
}

public class DeleteAuctionCommandHandler : IRequestHandler<DeleteAuctionCommand>
{
    private readonly IEntityRepository _repository;

    private readonly ILogger<DeleteAuctionCommandHandler> _logger;

    public DeleteAuctionCommandHandler(IEntityRepository repository, ILogger<DeleteAuctionCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<Auction>(request.Id)
            ?? throw new EntityNotFoundException("Auction cannot be found");

        if (auction.CreatorId != request.CreatorId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        if (auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot delete auction 5 minutes before its start");
        }

        await _repository.Remove<Auction>(request.Id);

        await _repository.SaveChanges();

        _logger.LogInformation($"[{DateTime.UtcNow}]-[{this.GetType().Name}] was executed successfully!");
    }
}
