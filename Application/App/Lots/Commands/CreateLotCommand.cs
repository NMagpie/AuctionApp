using Application.App.Lots.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.App.Lots.Commands;

public class CreateLotCommand : IRequest<LotDto>
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public int AuctionId { get; set; }

    public int UserId { get; set; }

    public decimal InitialPrice { get; set; }

    public HashSet<string> Categories { get; set; } = [];
}

public class CreateLotCommandHandler : IRequestHandler<CreateLotCommand, LotDto>
{

    private readonly IEntityRepository _repository;

    private readonly CreateLotCommandValidator _validator;

    private readonly IMapper _mapper;

    public CreateLotCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _validator = new CreateLotCommandValidator();
        _mapper = mapper;
    }

    public async Task<LotDto> Handle(CreateLotCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var auction = await _repository.GetById<Auction>(request.AuctionId)
            ?? throw new EntityNotFoundException("Auciton cannot be found");

        if (auction.CreatorId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        if (auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot edit lots of auction 5 minutes before its start");
        }

        var lot = _mapper.Map<CreateLotCommand, Lot>(request);

        await _repository.Add(lot);

        await _repository.SaveChanges();

        var lotDto = _mapper.Map<Lot, LotDto>(lot);

        return lotDto;
    }
}
