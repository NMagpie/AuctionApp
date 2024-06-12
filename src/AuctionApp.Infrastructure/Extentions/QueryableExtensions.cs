using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AuctionApp.Application.Extentions;

public static class QueryableExtensions
{
    public async static Task<PaginatedResult<TDto>> CreatePaginatedResultAsync<TEntity, TDto>(this IQueryable<TEntity> query, PagedRequest pagedRequest, IMapper mapper)
        where TEntity : class, IEntity
        where TDto : class
    {
        query = query.Filter(pagedRequest);

        var total = await query.CountAsync();

        var projectionResult = query.ProjectTo<TDto>(mapper.ConfigurationProvider);

        projectionResult = projectionResult.Sort(pagedRequest);

        projectionResult = projectionResult.Paginate(pagedRequest);

        var listResult = await projectionResult.ToListAsync();

        return new PaginatedResult<TDto>()
        {
            Items = listResult,
            PageSize = pagedRequest.PageSize,
            PageIndex = pagedRequest.PageIndex,
            Total = total
        };
    }

    private static IQueryable<T> Paginate<T>(this IQueryable<T> query, PagedRequest pagedRequest)
    {
        var entities = query.Skip(pagedRequest.PageIndex * pagedRequest.PageSize).Take(pagedRequest.PageSize);
        return entities;
    }

    private static IQueryable<T> Sort<T>(this IQueryable<T> query, PagedRequest pagedRequest)
    {
        if (!string.IsNullOrWhiteSpace(pagedRequest.ColumnNameForSorting))
        {
            query = query.OrderBy(pagedRequest.ColumnNameForSorting + " " + pagedRequest.SortDirection);
        }
        return query;
    }

    private static IQueryable<T> Filter<T>(this IQueryable<T> query, PagedRequest pagedRequest)
    {
        if (!string.IsNullOrWhiteSpace(pagedRequest.Filter))
        {
            query = query.Where(pagedRequest.Filter);
        }
        return query;
    }
}
