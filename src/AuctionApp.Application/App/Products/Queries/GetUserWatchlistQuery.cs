using Application.App.Products.Responses;
using AuctionApp.Application.Common.Abstractions;
using AuctionApp.Application.Common.Models;
using MediatR;

namespace AuctionApp.Application.App.Products.Queries;

public class GetUserWatchlistQuery : IPagedRequest, IRequest<PaginatedResult<ProductDto>>
{
    public int UserId { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}

public class GetUserWatchlistQueryHandler : IRequestHandler<GetUserWatchlistQuery, PaginatedResult<ProductDto>>
{

    private readonly IProductQueryRepository _productQueryRepository;

    public GetUserWatchlistQueryHandler(IProductQueryRepository productQueryRepository)
    {
        _productQueryRepository = productQueryRepository;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(GetUserWatchlistQuery request, CancellationToken cancellationToken)
    {
        return await _productQueryRepository.GetProductsByWatchlist<ProductDto>(request);
    }
}
