using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Users;
public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }

    public string Username { get; set; }
}
