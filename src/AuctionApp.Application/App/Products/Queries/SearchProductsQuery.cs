using Application.App.Products.Responses;
using Application.Common.Abstractions;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuctionApp.Application.App.Products.Queries;

public class SearchProductsQuery : IRequest<PaginatedResult<ProductDto>>
{
    public string? SearchQuery { get; set; }

    public int PageIndex { get; set; }

    public string? Category { get; set; }

    public string? ColumnNameForSorting { get; set; }

    public string? SortDirection { get; set; }
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
        var query = _mapper.Map<PagedRequest>(request);

        var filters = new List<string>();

        if (!string.IsNullOrEmpty(request.SearchQuery))
        {
            filters.Add($"Title.Contains(\"{request.SearchQuery}\") or Description.Contains(\"{request.SearchQuery}\")");
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            filters.Add($"Categories.Any(c => c.Name == \"{request.Category}\")");
        }

        query.Filter = string.Join(" and ", filters);

        query.PageSize = 12;

        var result = await _repository.GetPagedData<Product, ProductDto>(query);

        return result;
    }
}