using Application.App.Auctions.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
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
    private readonly IEntityRepository _entityRepository;

    private readonly IUserRepository _userRepository;

    private readonly ILogger<CreateAuctionCommandHandler> _logger;

    private readonly IMapper _mapper;

    public CreateAuctionCommandHandler(IEntityRepository entityRepository, IUserRepository userRepository, ILogger<CreateAuctionCommandHandler> logger, IMapper mapper)
    {
        _entityRepository = entityRepository;
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<AuctionDto> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.CreatorId)
            ?? throw new EntityNotFoundException("User cannot be found");

        var auction = _mapper.Map<CreateAuctionCommand, Auction>(request);

        auction.CreatorId = user.Id;

        await _entityRepository.Add(auction);

        await _entityRepository.SaveChanges();

        var auctionDto = _mapper.Map<Auction, AuctionDto>(auction);

        _logger.LogInformation($"[{DateTime.UtcNow}]-[{this.GetType().Name}] was executed successfully!");

        return auctionDto;
    }
}
