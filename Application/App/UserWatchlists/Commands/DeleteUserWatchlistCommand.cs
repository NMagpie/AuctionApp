using Application.Abstractions;
using Application.App.UserWatchlists.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.UserWatchlists.Commands;

public class DeleteWatchlistCommand : IRequest<UserWatchlistDto>
{
    public int Id { get; set; }
}

public class DeleteUserWatchlistCommandHandler : IRequestHandler<DeleteWatchlistCommand, UserWatchlistDto>
{
    private readonly IRepository _repository;

    public DeleteUserWatchlistCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserWatchlistDto> Handle(DeleteWatchlistCommand request, CancellationToken cancellationToken)
    {
        var userWatchlist = await _repository.Remove<UserWatchlist>(request.Id);

        await _repository.SaveChanges();

        var userWatchlistDto = UserWatchlistDto.FromUserWatchlist(userWatchlist);

        return userWatchlistDto;
    }
}
