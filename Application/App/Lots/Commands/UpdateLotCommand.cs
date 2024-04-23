using Application.Abstractions;
using Application.App.Lots.Responses;
using AuctionApp.Domain.Models;
using EntityFramework.Domain.Models;
using MediatR;

namespace Application.App.Lots.Commands;

public class UpdateLotCommand : IRequest<LotDto>
{
    public int Id { get; set; }
    public string? Title { get; set; }

    public string? Description { get; set; }

    public decimal? InitialPrice { get; set; }

    public HashSet<int>? Categories { get; set; } = [];
}

public class UpdateLotCommandHandler : IRequestHandler<UpdateLotCommand, LotDto>
{

    private readonly IRepository _repository;

    private readonly UpdateLotCommandValidator _validator;

    public UpdateLotCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new UpdateLotCommandValidator();
    }

    public async Task<LotDto> Handle(UpdateLotCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var lot = await _repository.GetById<Lot>(request.Id)
            ?? throw new ArgumentNullException("Lot cannot be found");

        if (lot.Auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new ArgumentException("Cannot edit lots of auction 5 minutes before its start");
        }

        var categories = (await _repository.GetByIds<Category>(request.Categories?.ToList() ?? []))
            .ToHashSet();

        lot.Title = request.Title ?? lot.Title;
        lot.Description = request.Description ?? lot.Description;
        lot.InitialPrice = request.InitialPrice ?? lot.InitialPrice;
        lot.Categories = categories ?? lot.Categories;

        await _repository.SaveChanges();

        var lotDto = LotDto.FromLot(lot);

        return lotDto;
    }
}
