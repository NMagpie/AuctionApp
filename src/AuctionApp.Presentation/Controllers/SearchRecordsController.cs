using AuctionApp.Application.App.SearchQueries.Commands;
using AuctionApp.Application.App.SearchQueries.Queries;
using AuctionApp.Application.App.SearchQueries.Responses;
using AuctionApp.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Swashbuckle.AspNetCore.Annotations;

namespace AuctionApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SearchRecordsController : AppBaseController
{
    private readonly IMediator _mediator;

    public SearchRecordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(OperationId = nameof(GetRecentSearches))]
    public async Task<ActionResult<PaginatedResult<SearchRecordDto>>> GetRecentSearches()
    {
        var userId = GetUserId();

        var recentSearches = await _mediator.Send(new GetRecentSearchRecordsQuery() { UserId = userId });

        return Ok(recentSearches);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(OperationId = nameof(DeleteSearchRecord))]
    public async Task<ActionResult> DeleteSearchRecord(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteSearchRecordCommand() { Id = id, UserId = userId });

        return Ok();
    }
}
