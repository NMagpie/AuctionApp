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
}

public class DeleteAuctionCommandHandler : IRequestHandler<DeleteAuctionCommand>
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

    public async Task Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<Auction>(request.Id)
            ?? throw new EntityNotFoundException("Auction cannot be found");

        if (auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot delete auction 5 minutes before its start");
        }

        _repository.Remove<Auction>(request.Id);

        await _repository.SaveChanges();

        _logger.LogInformation($"[{DateTime.UtcNow}]-[{this.GetType().Name}] was executed successfully!");
    }
}
