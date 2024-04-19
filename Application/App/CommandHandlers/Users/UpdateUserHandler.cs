using Application.Abstractions;
using Application.App.Commands.Users;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Users;
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
{

    private readonly IUnitOfWork _unitofWork;

    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
        _validator = new UpdateUserCommandValidator();
    }
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = await _unitofWork.Repository.GetById<User>(request.Id)
            ?? throw new ArgumentNullException("Use rcannot be found");

        user.Username = request.Username;

        await _unitofWork.SaveChanges();

        var userDto = UserDto.FromUser(user);

        return userDto;
    }
}
