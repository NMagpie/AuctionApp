using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Queries;
public class GetLatestProductPriceByIdQuery : IRequest<decimal>
{
    public int Id { get; set; }
}

public class GetLatestProductPriceByIdQueryHandler : IRequestHandler<GetLatestProductPriceByIdQuery, decimal>
{
    private readonly IEntityRepository _repository;

    public GetLatestProductPriceByIdQueryHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<decimal> Handle(GetLatestProductPriceByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdWithInclude<Product>(request.Id, product => product.Creator, product => product.Bids)
            ?? throw new EntityNotFoundException("Product cannot be found");

        var price = product.Bids.Count == 0 ? product.InitialPrice : product.Bids.Max(bid => bid.Amount);

        return price;
    }
}
