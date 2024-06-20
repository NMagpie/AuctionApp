using Application.App.Products.Responses;
using AuctionApp.Application.Common.Abstractions;
using AuctionApp.Application.Common.Models;
using AutoMapper;
using MediatR;

namespace AuctionApp.Application.App.Products.Queries;

public class SearchProductsQuery : IPagedRequest, IRequest<PaginatedResult<ProductDto>>
{
    public string? SearchQuery { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public string? Category { get; set; }

    public ProductSearchPresets? SearchPreset { get; set; }

    public decimal? MaxPrice { get; set; }

    public decimal? MinPrice { get; set; }

    public string? ColumnNameForSorting { get; set; }

    public string? SortDirection { get; set; }

    public int? CreatorId { get; set; }
}

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, PaginatedResult<ProductDto>>
{
    private readonly IProductQueryRepository _repository;

    private readonly IMapper _mapper;

    public SearchProductsQueryHandler(IProductQueryRepository productQueryRepository, IMapper mapper)
    {
        _repository = productQueryRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedProductData<ProductDto>(request);

        return result;
    }
}
