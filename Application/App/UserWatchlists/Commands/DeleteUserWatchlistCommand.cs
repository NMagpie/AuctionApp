using Application.Abstractions;
using Application.App.UserWatchlists.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.UserWatchlists.Commands;

public class DeleteUserWatchlistCommand : IRequest<UserWatchlistDto>
{
    public int Id { get; set; }
}

public class DeleteUserWatchlistCommandHandler : IRequestHandler<DeleteUserWatchlistCommand, UserWatchlistDto>
{
    private readonly IRepository _repository;

    public DeleteUserWatchlistCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserWatchlistDto> Handle(DeleteUserWatchlistCommand request, CancellationToken cancellationToken)
    {
        var userWatchlist = await _repository.Remove<UserWatchlist>(request.Id);

        await _repository.SaveChanges();

        var userWatchlistDto = UserWatchlistDto.FromUserWatchlist(userWatchlist);

        return userWatchlistDto;
    }
}
