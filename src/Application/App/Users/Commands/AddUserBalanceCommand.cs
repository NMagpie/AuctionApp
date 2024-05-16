using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AutoMapper;
using Domain.Auth;
using MediatR;

namespace Application.App.Users.Commands;
public class AddUserBalanceCommand : IRequest<UserDto>
{
    public int Id { get; set; }

    public decimal Amount { get; set; }
}

public class AddUserBalanceCommandHandler : IRequestHandler<AddUserBalanceCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    public AddUserBalanceCommandHandler(IUserRepository repository, IMapper mapper)
    {
        _userRepository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.Id)
            ?? throw new EntityNotFoundException("User cannot be found");

        user.Balance += request.Amount;

        await _userRepository.SaveChanges();

        var userDto = _mapper.Map<User, UserDto>(user);

        return userDto;
    }
}
