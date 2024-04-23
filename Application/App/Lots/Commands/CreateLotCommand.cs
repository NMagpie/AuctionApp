using Application.Abstractions;
using Application.App.Lots.Responses;
using AuctionApp.Domain.Models;
using EntityFramework.Domain.Models;
using MediatR;

namespace Application.App.Lots.Commands;

public class CreateLotCommand : IRequest<LotDto>
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public int AuctionId { get; set; }

    public decimal InitialPrice { get; set; }

    public HashSet<int> Categories { get; set; } = [];
}

public class CreateLotCommandHandler : IRequestHandler<CreateLotCommand, LotDto>
{

    private readonly IRepository _repository;

    private readonly CreateLotCommandValidator _validator;

    public CreateLotCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new CreateLotCommandValidator();
    }

    public async Task<LotDto> Handle(CreateLotCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var auction = await _repository.GetById<Auction>(request.AuctionId)
            ?? throw new ArgumentNullException("Auciton cannot be found");

        var categories = (await _repository.GetByIds<Category>(request.Categories.ToList()))
            .ToHashSet();

        var lot = new Lot()
        {
            Title = request.Title,
            Description = request.Description ?? "",
            AuctionId = auction.Id,
            InitialPrice = request.InitialPrice,
            Categories = categories
        };

        await _repository.Add(lot);

        await _repository.SaveChanges();

        var lotDto = LotDto.FromLot(lot);

        return lotDto;
    }
}
