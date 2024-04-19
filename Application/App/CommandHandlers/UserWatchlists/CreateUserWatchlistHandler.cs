using Application.Abstractions;
using Application.App.Commands.UserWatchlist;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.UserWatchlists;
public class CreateUserWatchlistHandler : IRequestHandler<CreateUserWatchlistCommand, UserWatchlistDto>
{
    private readonly IUnitOfWork _unitofWork;

    private readonly CreateUserWatchlistCommandValidator _validator;

    public CreateUserWatchlistHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
        _validator = new CreateUserWatchlistCommandValidator();
    }

    public async Task<UserWatchlistDto> Handle(CreateUserWatchlistCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = await _unitofWork.Repository.GetById<User>(request.UserId)
            ?? throw new ArgumentNullException("User cannot be found");

        var auction = await _unitofWork.Repository.GetById<Auction>(request.AuctionId)
            ?? throw new ArgumentNullException("Auction cannot be found");

        var userWatchlist = new UserWatchlist()
        {
            UserId = request.UserId,
            User = user,
            AuctionId = request.AuctionId,
            Auction = auction,
        };

        await _unitofWork.Repository.Add(userWatchlist);

        var userWatchlistDto = UserWatchlistDto.FromUserWatchlist(userWatchlist);

        return userWatchlistDto;
    }
}
