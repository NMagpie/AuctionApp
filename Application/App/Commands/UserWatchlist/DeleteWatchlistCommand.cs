using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.UserWatchlist;
public class DeleteWatchlistCommand : IRequest<UserWatchlistDto>
{
    public int Id { get; set; }
}
