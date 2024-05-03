using Application.Abstractions;
using Application.App.Auctions.Responses;
using Application.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.App.Auctions.Commands;

public class CreateAuctionCommand : IRequest<AuctionDto>
{
    public string Title { get; set; }

    public int CreatorId { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset EndTime { get; set; }

    public List<LotInAuctionDto> Lots { get; set; } = [];
}

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, AuctionDto>
{
    private readonly IRepository _repository;

    private readonly CreateAuctionCommandValidator _validator;

    private readonly ILogger<CreateAuctionCommandHandler> _logger;

    private readonly IMapper _mapper;

    public CreateAuctionCommandHandler(IRepository repository, ILogger<CreateAuctionCommandHandler> logger, IMapper mapper)
    {
        _repository = repository;
        _validator = new CreateAuctionCommandValidator();
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<AuctionDto> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var user = await _repository.GetById<User>(request.CreatorId)
            ?? throw new ArgumentNullException("User cannot be found");

        var auction = _mapper.Map<CreateAuctionCommand, Auction>(request);

        await _repository.Add(auction);

        await _repository.SaveChanges();

        var auctionDto = _mapper.Map<Auction, AuctionDto>(auction);

        _logger.LogInformation($"[{DateTime.UtcNow}]-[{this.GetType().Name}] was executed successfully!");

        return auctionDto;
    }
}
