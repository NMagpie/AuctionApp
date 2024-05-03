using Application.Abstractions;
using Application.App.Users.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById<User>(request.Id)
            ?? throw new ArgumentNullException("User cannot be found");

        var userDto = _mapper.Map<User, UserDto>(user);

        return userDto;
    }
}
