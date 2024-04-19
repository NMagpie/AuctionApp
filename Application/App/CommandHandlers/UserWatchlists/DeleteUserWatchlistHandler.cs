using Application.Abstractions;
using Application.App.Commands.UserWatchlist;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.UserWatchlists;
public class DeleteUserWatchlistHandler : IRequestHandler<DeleteWatchlistCommand, UserWatchlistDto>
{
    private readonly IUnitOfWork _unitofWork;

    public DeleteUserWatchlistHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
    }

    public async Task<UserWatchlistDto> Handle(DeleteWatchlistCommand request, CancellationToken cancellationToken)
    {
        var userWatchlist = await _unitofWork.Repository.Remove<UserWatchlist>(request.Id);

        await _unitofWork.SaveChanges();

        var userWatchlistDto = UserWatchlistDto.FromUserWatchlist(userWatchlist);

        return userWatchlistDto;
    }
}
