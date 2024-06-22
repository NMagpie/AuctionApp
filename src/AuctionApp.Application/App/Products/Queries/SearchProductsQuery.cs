using Application.App.Products.Responses;
using Application.Common.Abstractions;
using AuctionApp.Application.Common.Abstractions;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
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

    public int? UserId { get; set; }
}

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, PaginatedResult<ProductDto>>
{
    private readonly IEntityRepository _entityRepository;

    private readonly IProductQueryRepository _productQueryRepository;

    public SearchProductsQueryHandler(IEntityRepository entityRepository, IProductQueryRepository productQueryRepository)
    {
        _entityRepository = entityRepository;
        _productQueryRepository = productQueryRepository;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId != null && !string.IsNullOrEmpty(request.SearchQuery))
        {
            var searchRecord = (await _entityRepository.GetByPredicate<SearchRecord>(
                r => r.UserId == request.UserId &&
                r.SearchQuery == request.SearchQuery
                )).FirstOrDefault();

            if (searchRecord == null)
            {
                searchRecord = new SearchRecord()
                {
                    UserId = request.UserId.Value,
                    SearchQuery = request.SearchQuery,
                    LastUserAt = DateTimeOffset.UtcNow,
                };

                await _entityRepository.Add(searchRecord);
            }
            else
            {
                searchRecord.LastUserAt = DateTimeOffset.UtcNow;
            }

            await _entityRepository.SaveChanges();
        }

        var result = await _productQueryRepository.GetPagedProductData<ProductDto>(request);

        return result;
    }
}
