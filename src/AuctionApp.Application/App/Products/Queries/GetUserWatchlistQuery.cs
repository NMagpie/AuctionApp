using Application.App.Products.Responses;
using Application.Common.Abstractions;
using AuctionApp.Application.Common.Abstractions;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
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

    private readonly IEntityRepository _repository;

    private readonly IProductQueryRepository _productQueryRepository;

    private readonly IMapper _mapper;

    public GetUserWatchlistQueryHandler(IEntityRepository repository, IProductQueryRepository productQueryRepository, IMapper mapper)
    {
        _repository = repository;
        _productQueryRepository = productQueryRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(GetUserWatchlistQuery request, CancellationToken cancellationToken)
    {
        var products = await _productQueryRepository.GetProductsByWatchlist<ProductDto>(request);

        return products;
    }
}
