using Application.Common.Exceptions;
using AuctionApp.Application.App.Products.Queries;
using AuctionApp.Application.Common.Abstractions;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AuctionApp.Infrastructure.Persistance.Repositories;

public class ProductQueryRepository : IProductQueryRepository
{

    private readonly AuctionAppDbContext _auctionAppDbContext;

    private readonly IMapper _mapper;

    public ProductQueryRepository(AuctionAppDbContext auctionAppDbContext, IMapper mapper)
    {
        _auctionAppDbContext = auctionAppDbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<TDto>> GetPagedProductData<TDto>(SearchProductsQuery requestQuery)
        where TDto : class
    {
        IQueryable<Product> query = _auctionAppDbContext
            .Set<Product>()
            .AsQueryable();

        query = Filter(requestQuery, query);

        var total = await query.CountAsync();

        var projectionResult = query.ProjectTo<TDto>(_mapper.ConfigurationProvider);

        projectionResult = Sort(requestQuery, projectionResult);

        projectionResult = Paginate(requestQuery, projectionResult);

        var listResult = await projectionResult.ToListAsync();

        return new PaginatedResult<TDto>()
        {
            Items = listResult,
            PageSize = requestQuery.PageSize,
            PageIndex = requestQuery.PageIndex,
            Total = total
        };
    }

    public async Task<PaginatedResult<TDto>> GetProductsByWatchlist<TDto>(GetUserWatchlistQuery requestQuery)
    where TDto : class
    {
        var watchlistQuery = _auctionAppDbContext
            .Set<UserWatchlist>()
            .OrderByDescending(x => x.Created);

        var total = await watchlistQuery.CountAsync();

        watchlistQuery
            .Skip(requestQuery.PageIndex * requestQuery.PageSize)
            .Take(requestQuery.PageSize)
            .Select(w => new { w.ProductId, w.Created });

        var query = _auctionAppDbContext
            .Set<Product>()
            .Join(watchlistQuery,
                  product => product.Id,
                  watchlist => watchlist.ProductId,
                  (product, watchlist) => new { Product = product, watchlist.Created })
            .OrderByDescending(p => p.Created)
            .Select(p => p.Product);

        var projectionResult = query.ProjectTo<TDto>(_mapper.ConfigurationProvider);

        projectionResult = Paginate(requestQuery, projectionResult);

        var listResult = await projectionResult.ToListAsync();

        return new PaginatedResult<TDto>()
        {
            Items = listResult,
            PageSize = requestQuery.PageSize,
            PageIndex = requestQuery.PageIndex,
            Total = total
        };
    }

    public async Task<PaginatedResult<TDto>> GetProductsUserParticipated<TDto>(GetProductsUserParticipatedQuery requestQuery)
    where TDto : class
    {
        var query = _auctionAppDbContext
            .Set<Product>()
            .Where(p => p.Bids.Any(b => b.UserId == requestQuery.UserId))
            .Select(p => new
            {
                Product = p,
                LatestBidTime = p.Bids.Any() ? p.Bids.Max(b => b.CreateTime) : DateTimeOffset.MinValue
            })
            .OrderByDescending(x => x.LatestBidTime)
            .Select(x => x.Product);

        var total = await query.CountAsync();

        var projectionResult = query.ProjectTo<TDto>(_mapper.ConfigurationProvider);

        projectionResult = Paginate(requestQuery, projectionResult);

        var listResult = await projectionResult.ToListAsync();

        return new PaginatedResult<TDto>()
        {
            Items = listResult,
            PageSize = requestQuery.PageSize,
            PageIndex = requestQuery.PageIndex,
            Total = total
        };
    }

    private IQueryable<Product> Filter(SearchProductsQuery requestQuery, IQueryable<Product> query)
    {
        if (!string.IsNullOrEmpty(requestQuery.SearchQuery))
        {
            query = query.Where(p => p.Title.Contains(requestQuery.SearchQuery) || p.Description.Contains(requestQuery.SearchQuery));
        }

        if (!string.IsNullOrEmpty(requestQuery.Category))
        {
            query = query.Where(p => p.Category.Name == requestQuery.Category);
        }

        if (requestQuery.MaxPrice != null)
        {
            query = query.Where(p => p.InitialPrice <= requestQuery.MaxPrice);
        }

        if (requestQuery.MinPrice != null)
        {
            query = query.Where(p => p.InitialPrice >= requestQuery.MinPrice);
        }

        if (requestQuery.CreatorId != null)
        {
            query = query.Where(p => p.CreatorId == requestQuery.CreatorId);
        }

        if (requestQuery.SearchPreset.HasValue)
        {
            query = GetFilteringByPreset(requestQuery, query);
        }
        else
        {
            query = query.OrderByDescending(p => p.StartTime);
        }

        return query;
    }

    private IQueryable<Product> GetFilteringByPreset(SearchProductsQuery requestQuery, IQueryable<Product> query)
    {
        var currentTime = DateTimeOffset.UtcNow;

        var nearestTime = currentTime.AddMinutes(1);

        query = requestQuery.SearchPreset switch
        {
            ProductSearchPresets.ComingSoon => query.Where(p => p.StartTime >= nearestTime)
                                .OrderBy(p => p.StartTime),

            ProductSearchPresets.EndingSoon => query.Where(p => p.StartTime <= currentTime && p.EndTime >= nearestTime)
                                .OrderBy(p => p.EndTime),

            ProductSearchPresets.MostActive => query.Where(p => p.StartTime <= currentTime && p.EndTime >= nearestTime)
                                .OrderByDescending(p => p.Bids.Count),

            ProductSearchPresets.BidHigh => query.Where(p => p.StartTime <= currentTime && p.EndTime >= nearestTime)
                                .OrderByDescending(p => p.Bids.Select(b => b.Amount).DefaultIfEmpty().Max()),

            ProductSearchPresets.BidLow => query.Where(p => p.StartTime <= currentTime && p.EndTime >= nearestTime)
                                .OrderBy(p => p.Bids.Select(b => b.Amount).DefaultIfEmpty().Max()),

            _ => throw new BusinessValidationException("Invalid search preset"),
        };

        return query;
    }

    private IQueryable<TDto> Sort<TDto>(SearchProductsQuery requestQuery, IQueryable<TDto> query)
    {
        if (!string.IsNullOrWhiteSpace(requestQuery.ColumnNameForSorting))
        {
            query = query.OrderBy($"{requestQuery.ColumnNameForSorting} {requestQuery.SortDirection}");
        }

        return query;
    }

    private IQueryable<TDto> Paginate<TDto>(IPagedRequest requestQuery, IQueryable<TDto> query)
    {
        query = query.Skip(requestQuery.PageIndex * requestQuery.PageSize).Take(requestQuery.PageSize);

        return query;
    }
}
