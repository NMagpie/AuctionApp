using Application.Abstractions;
using Application.App.Commands.Users;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Users;
public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUnitOfWork _unitofWork;

    private readonly CreateUserCommandValidator _validator;

    public CreateUserHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
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

        await _unitofWork.Repository.Add(user);

        await _unitofWork.SaveChanges();

        var userDto = UserDto.FromUser(user);

        return userDto;
    }
}
