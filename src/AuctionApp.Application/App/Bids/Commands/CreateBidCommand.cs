using Application.App.Bids.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Domain.Auth;
using MediatR;

namespace Application.App.Bids.Commands;

public class CreateBidCommand : IRequest<BidDto>
{
    public required int ProductId { get; set; }

    public required int UserId { get; set; }

    public required decimal Amount { get; set; }
}

public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand, BidDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public CreateBidCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BidDto> Handle(CreateBidCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById<User>(request.UserId)
            ?? throw new EntityNotFoundException("User cannot be found");

        var userReservedBalance =
            (await _repository.GetByPredicate<Bid>(
                b => b.UserId == user.Id &&
                b.ProductId != request.ProductId &&
                b.IsWon &&
                !b.Product.SellingFinished
                ))
            .Sum(b => b.Amount);

        if (userReservedBalance + request.Amount > user.Balance)
        {
            throw new BusinessValidationException("Cannot place bid: unsufficient balance");
        }

        var product = await _repository.GetByIdWithInclude<Product>(request.ProductId, product => product.Bids)
            ?? throw new EntityNotFoundException("Product cannot be found");

        if (product.EndTime <= DateTimeOffset.UtcNow)
        {
            throw new BusinessValidationException("Cannot place bid: Auction Time is out");
        }

        if (product.Bids.Select(b => b.Amount).Append(product.InitialPrice).Max() >= request.Amount)
        {
            throw new BusinessValidationException("Cannot place bid: your bid is lower than the current price");
        }

        var bid = _mapper.Map<CreateBidCommand, Bid>(request);

        foreach (var existingBid in product.Bids)
        {
            existingBid.IsWon = false;
        }

        bid.CreateTime = DateTimeOffset.UtcNow;

        bid.IsWon = true;

        await _repository.Add(bid);

        await _repository.SaveChanges();

        var bidDto = _mapper.Map<Bid, BidDto>(bid);

        return bidDto;
    }
}
