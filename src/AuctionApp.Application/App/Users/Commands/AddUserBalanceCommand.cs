using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Domain.Auth;
using MediatR;

namespace Application.App.Users.Commands;
public class AddUserBalanceCommand : IRequest<CurrentUserDto>
{
    public int Id { get; set; }

    public decimal Amount { get; set; }
}

public class AddUserBalanceCommandHandler : IRequestHandler<AddUserBalanceCommand, CurrentUserDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public AddUserBalanceCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CurrentUserDto> Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById<User>(request.Id)
            ?? throw new EntityNotFoundException("User cannot be found");

        user.Balance += request.Amount;

        var userReservedBalance =
            (await _repository.GetByPredicate<Bid>(b => b.UserId == user.Id && b.IsWon && !b.Product.SellingFinished))
            .Sum(b => b.Amount);

        await _repository.SaveChanges();

        var userDto = _mapper.Map<User, CurrentUserDto>(user);

        userDto.ReservedBalance = userReservedBalance;

        return userDto;
    }
}
