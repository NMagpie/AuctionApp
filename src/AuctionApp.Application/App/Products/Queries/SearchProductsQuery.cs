using Application.App.Products.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuctionApp.Application.App.Products.Queries;

public class SearchProductsQuery : IRequest<PaginatedResult<ProductDto>>
{
    public string? SearchQuery { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public string? Category { get; set; }

    public EProductSearchPresets? SearchPreset { get; set; }

    public decimal? MaxPrice { get; set; }

    public decimal? MinPrice { get; set; }

    public string? ColumnNameForSorting { get; set; }

    public string? SortDirection { get; set; }

    public int? CreatorId { get; set; }
}

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, PaginatedResult<ProductDto>>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public SearchProductsQueryHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var filters = new List<string>();

        if (!string.IsNullOrEmpty(request.SearchQuery))
        {
            filters.Add($"(Title.Contains(\"{request.SearchQuery}\") or Description.Contains(\"{request.SearchQuery}\"))");
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            filters.Add($"Category.Name == \"{request.Category}\"");
        }

        if (request.MaxPrice != null)
        {
            filters.Add($"InitialPrice <= {request.MaxPrice}");
        }

        if (request.MinPrice != null)
        {
            filters.Add($"InitialPrice >= {request.MinPrice}");
        }

        if (request.CreatorId != null)
        {
            filters.Add($"CreatorId == {request.CreatorId}");
        }

        if (request.SearchPreset.HasValue)
        {
            GetFilteringByPreset(request, filters);
        }

        var query = _mapper.Map<PagedRequest>(request);

        query.Filter = string.Join(" and ", filters);

        var result = await _repository.GetPagedData<Product, ProductDto>(query);

        return result;
    }

    private void GetFilteringByPreset(SearchProductsQuery request, List<string> filters)
    {
        var currentTime = DateTimeOffset.UtcNow;

        var nearestTime = currentTime.AddMinutes(1);

        switch (request.SearchPreset)
        {
            case EProductSearchPresets.ComingSoon:
                filters.Add($"StartTime >= DateTimeOffset(\"{nearestTime}\")");
                request.ColumnNameForSorting = "StartTime";
                request.SortDirection = "asc";
                break;

            case EProductSearchPresets.EndingSoon:
                filters.Add($"StartTime <= DateTimeOffset(\"{currentTime}\") and EndTime >= DateTimeOffset(\"{nearestTime}\")");
                request.ColumnNameForSorting = "EndTime";
                request.SortDirection = "asc";
                break;

            case EProductSearchPresets.MostActive:
                filters.Add($"StartTime <= DateTimeOffset(\"{currentTime}\") and EndTime >= DateTimeOffset(\"{nearestTime}\")");
                request.ColumnNameForSorting = "Bids.Count";
                request.SortDirection = "desc";
                break;

            case EProductSearchPresets.BidHigh:
                filters.Add($"StartTime <= DateTimeOffset(\"{currentTime}\") and EndTime >= DateTimeOffset( \"{nearestTime}\")");
                request.ColumnNameForSorting = "Bids.Select(b => b.Amount).DefaultIfEmpty().Max()";
                request.SortDirection = "desc";
                break;

            case EProductSearchPresets.BidLow:
                filters.Add($"StartTime <= DateTimeOffset(\"{currentTime}\") and EndTime >= DateTimeOffset(\"{nearestTime}\")");
                request.ColumnNameForSorting = "Bids.Select(b => b.Amount).DefaultIfEmpty().Max()";
                request.SortDirection = "asc";
                break;

            default:
                throw new BusinessValidationException("Invalid search preset");
        };
    }
}
