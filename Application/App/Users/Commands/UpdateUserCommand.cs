using Application.Abstractions;
using Application.App.Users.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Users.Commands;

public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }

    public string Username { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{

    private readonly IRepository _repository;

    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new UpdateUserCommandValidator();
    }
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = await _repository.GetById<User>(request.Id)
            ?? throw new ArgumentNullException("Use rcannot be found");

        user.Username = request.Username;

        await _repository.SaveChanges();

        var userDto = UserDto.FromUser(user);

        return userDto;
    }
}
