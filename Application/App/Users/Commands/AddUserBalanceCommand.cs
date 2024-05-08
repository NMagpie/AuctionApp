using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.App.Users.Commands;
public class AddUserBalanceCommand : IRequest<UserDto>
{
    public int Id { get; set; }

    public decimal Amount { get; set; }
}

public class AddUserBalanceCommandHandler : IRequestHandler<AddUserBalanceCommand, UserDto>
{
    private readonly IRepository _repository;

    private readonly AddUserBalanceCommandValidator _validator;

    private readonly IMapper _mapper;

    public AddUserBalanceCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _validator = new AddUserBalanceCommandValidator();
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var user = await _repository.GetById<User>(request.Id)
            ?? throw new EntityNotFoundException("User cannot be found");

        user.Balance += request.Amount;

        await _repository.SaveChanges();

        var userDto = _mapper.Map<User, UserDto>(user);

        return userDto;
    }
}
