using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Domain.Auth;
using MediatR;

namespace Application.App.Queries;
public class GetCurrentUserQuery : IRequest<CurrentUserDto>
{
    public int Id { get; set; }
}

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public GetCurrentUserHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById<User>(request.Id)
            ?? throw new EntityNotFoundException("User cannot be found");

        var userReservedBalance =
            (await _repository.GetByPredicate<Bid>(b => b.UserId == user.Id && b.IsWon && !b.Product.SellingFinished))
            .Sum(b => b.Amount);

        var userDto = _mapper.Map<User, CurrentUserDto>(user);

        userDto.ReservedBalance = userReservedBalance;

        return userDto;
    }
}
