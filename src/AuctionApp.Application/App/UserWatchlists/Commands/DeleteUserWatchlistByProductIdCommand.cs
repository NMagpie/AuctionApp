namespace AuctionApp.Application.App.UserWatchlists.Commands;

using AuctionApp.Domain.Models;
using global::Application.Common.Abstractions;
using global::Application.Common.Exceptions;
using MediatR;

public class DeleteUserWatchlistByProductIdCommand : IRequest
{
    public int ProductId { get; set; }

    public int UserId { get; set; }
}

public class DeleteUserWatchlistByProductIdCommandHandler : IRequestHandler<DeleteUserWatchlistByProductIdCommand>
{
    private readonly IEntityRepository _repository;

    public DeleteUserWatchlistByProductIdCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteUserWatchlistByProductIdCommand request, CancellationToken cancellationToken)
    {
        var userWatchlist = (await _repository.GetByPredicate<UserWatchlist>(uw => uw.ProductId == request.ProductId && uw.UserId == request.UserId)).FirstOrDefault()
            ?? throw new EntityNotFoundException("User watchlist cannot be found");

        if (userWatchlist.UserId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        await _repository.Remove<UserWatchlist>(userWatchlist.Id);

        await _repository.SaveChanges();
    }
}
