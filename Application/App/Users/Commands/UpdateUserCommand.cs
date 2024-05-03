using Application.Abstractions;
using Application.App.Users.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
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

    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _validator = new UpdateUserCommandValidator();
        _mapper = mapper;
    }
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = await _repository.GetById<User>(request.Id)
            ?? throw new ArgumentNullException("Use rcannot be found");

        _mapper.Map(request, user);

        await _repository.SaveChanges();

        var userDto = _mapper.Map<User, UserDto>(user);

        return userDto;
    }
}
