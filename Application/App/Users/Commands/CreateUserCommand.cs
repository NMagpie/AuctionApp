using Application.Abstractions;
using Application.App.Users.Responses;
using AuctionApp.Domain.Models;
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

    public CreateUserCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new CreateUserCommandValidator();
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = new User()
        {
            Username = request.Username,
            Balance = 0
        };

        await _repository.Add(user);

        await _repository.SaveChanges();

        var userDto = UserDto.FromUser(user);

        return userDto;
    }
}
