using Application.Abstractions;
using Application.App.Users.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Queries;
public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IRepository _repository;

    public GetUserByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<User>(request.Id)
            ?? throw new ArgumentNullException("User cannot be found");

        var auctionDto = UserDto.FromUser(auction);

        return auctionDto;
    }
}
