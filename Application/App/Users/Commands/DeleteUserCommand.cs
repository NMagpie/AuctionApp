using Application.Common.Abstractions;
using MediatR;

namespace Application.App.Users.Commands;

public class DeleteUserCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{

    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.Remove(request.Id);

        await _userRepository.SaveChanges();
    }
}
