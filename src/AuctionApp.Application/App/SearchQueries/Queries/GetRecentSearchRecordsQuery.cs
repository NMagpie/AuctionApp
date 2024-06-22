using Application.Common.Abstractions;
using AuctionApp.Application.App.SearchQueries.Responses;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.App.SearchQueries.Queries;

public class GetRecentSearchRecordsQuery : IRequest<PaginatedResult<SearchRecordDto>>
{
    public int UserId { get; set; }
}

public class GetRecentSearchRecordsQueryHandler : IRequestHandler<GetRecentSearchRecordsQuery, PaginatedResult<SearchRecordDto>>
{

    private readonly IEntityRepository _repository;

    public GetRecentSearchRecordsQueryHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResult<SearchRecordDto>> Handle(GetRecentSearchRecordsQuery request, CancellationToken cancellationToken)
    {
        var pagedRequest = new PagedRequest()
        {
            PageSize = 10,
            PageIndex = 0,
            ColumnNameForSorting = "LastUserAt",
            SortDirection = "desc",
            Filter = $"UserId == \"{request.UserId}\""
        };

        var recentRecords = await _repository.GetPagedData<SearchRecord, SearchRecordDto>(pagedRequest);

        return recentRecords;
    }
}