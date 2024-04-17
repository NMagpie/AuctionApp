using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Lots;
public class DeleteLotCommand : IRequest<LotDto>
{
    public int Id { get; set; }
}
