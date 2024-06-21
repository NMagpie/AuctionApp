using AuctionApp.Application.App.Products.Queries;
using AuctionApp.Application.Common.Models;

namespace AuctionApp.Application.Common.Abstractions;

public interface IProductQueryRepository
{
    Task<PaginatedResult<TDto>> GetPagedProductData<TDto>(SearchProductsQuery requestQuery)
            where TDto : class;

    Task<PaginatedResult<TDto>> GetProductsByWatchlist<TDto>(GetUserWatchlistQuery requestQuery)
            where TDto : class;

    Task<PaginatedResult<TDto>> GetProductsUserParticipated<TDto>(GetProductsUserParticipatedQuery requestQuery)
            where TDto : class;
}
