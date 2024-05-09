using Application.App.Lots.Commands;
using Application.App.Lots.Responses;
using Application.App.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class LotsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LotsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LotDto>> GetLot(int id)
    {
        var lotDto = await _mediator.Send(new GetLotByIdQuery() { Id = id });

        return Ok(lotDto);
    }

    [HttpPost]
    public async Task<ActionResult<LotDto>> CreateLot(CreateLotCommand createLotCommand)
    {
        var lotDto = await _mediator.Send(createLotCommand);

        return Ok(lotDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LotDto>> UpdateLot(UpdateLotCommand updateLotCommand)
    {
        var lotDto = await _mediator.Send(updateLotCommand);

        return Ok(lotDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLot(int id)
    {
        await _mediator.Send(new DeleteLotCommand() { Id = id });

        return Ok();
    }
}