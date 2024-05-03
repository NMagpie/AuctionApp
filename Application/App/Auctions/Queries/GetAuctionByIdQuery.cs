using Application.Abstractions;
using Application.App.Auctions.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.App.Auctions.Queries;
public class GetAuctionByIdQuery : IRequest<AuctionDto>
{
    public int Id { get; set; }
}

public class GetAuctionByIdQueryHandler : IRequestHandler<GetAuctionByIdQuery, AuctionDto>
{
    private readonly IRepository _repository;

    private readonly ILogger<GetAuctionByIdQueryHandler> _logger;

    private readonly IMapper _mapper;

    public GetAuctionByIdQueryHandler(IRepository repository, ILogger<GetAuctionByIdQueryHandler> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<AuctionDto> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<Auction>(request.Id)
            ?? throw new ArgumentNullException("Auction cannot be found");

        var auctionDto = _mapper.Map<Auction, AuctionDto>(auction);

        _logger.LogInformation($"[{DateTime.UtcNow}]-[{this.GetType().Name}] was executed successfully!");

        return auctionDto;
    }
}