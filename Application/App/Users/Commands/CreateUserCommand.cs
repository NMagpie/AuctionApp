using Application.App.Users.Responses;
using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.App.Users.Commands;

public class CreateUserCommand : IRequest<UserDto>
{
    public string Username { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IRepository _repository;

    private readonly CreateUserCommandValidator _validator;

    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _validator = new CreateUserCommandValidator();
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var user = _mapper.Map<CreateUserCommand, User>(request);

        user.Balance = 0;

        await _repository.Add(user);

        await _repository.SaveChanges();

        var userDto = _mapper.Map<User, UserDto>(user);

        return userDto;
    }
}
