using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using Domain.Auth;
using MediatR;

namespace AuctionApp.Application.App.ProductReviews.Queries;

public class CanUserLeaveReviewQuery : IRequest<bool>
{
    public int UserId { get; set; }

    public int ProductId { get; set; }
}

public class CanUserLeaveReviewQueryHandler : IRequestHandler<CanUserLeaveReviewQuery, bool>
{
    private readonly IEntityRepository _repository;

    public CanUserLeaveReviewQueryHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CanUserLeaveReviewQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdWithInclude<Product>(request.ProductId, p => p.Bids)
            ?? throw new EntityNotFoundException("Product cannot be found");

        var user = await _repository.GetByIdWithInclude<User>(request.UserId)
            ?? throw new EntityNotFoundException("User cannot be found");

        var isSellingFinished = product.EndTime < DateTimeOffset.UtcNow;

        var userHasPlacedBid = product.Bids.Any(b => b.UserId == request.UserId);

        return userHasPlacedBid && isSellingFinished;
    }
}