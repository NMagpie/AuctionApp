using Application.App.Auctions.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.App.Auctions.Commands;

public class UpdateAuctionCommand : IRequest<AuctionDto>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset EndTime { get; set; }
}

public class UpdateAuctionCommandHandler : IRequestHandler<UpdateAuctionCommand, AuctionDto>
{
    private readonly IRepository _repository;

    private readonly UpdateAuctionCommandValidator _validator;

    private readonly ILogger<UpdateAuctionCommandHandler> _logger;

    private readonly IMapper _mapper;

    public UpdateAuctionCommandHandler(IRepository repository, ILogger<UpdateAuctionCommandHandler> logger, IMapper mapper)
    {
        _repository = repository;
        _validator = new UpdateAuctionCommandValidator();
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<AuctionDto> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var auction = await _repository.GetById<Auction>(request.Id)
            ?? throw new EntityNotFoundException("Auction cannot be found");

        if (auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot edit auction 5 minutes before its start");
        }

        _mapper.Map(request, auction);

        await _repository.SaveChanges();

        var auctionDto = _mapper.Map<Auction, AuctionDto>(auction);

        _logger.LogInformation($"[{DateTime.UtcNow}]-[{this.GetType().Name}] was executed successfully!");

        return auctionDto;
    }
}
