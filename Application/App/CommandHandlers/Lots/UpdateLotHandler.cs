using Application.Abstractions;
using Application.App.Commands.Lots;
using Application.App.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using EntityFramework.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Lots;
public class UpdateLotHandler : IRequestHandler<UpdateLotCommand, LotDto>
{

    private readonly IUnitOfWork _unitofWork;

    private readonly UpdateLotCommandValidator _validator;

    public UpdateLotHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
        _validator = new UpdateLotCommandValidator();
    }

    public async Task<LotDto> Handle(UpdateLotCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var lot = await _unitofWork.Repository.GetById<Lot>(request.Id)
            ?? throw new ArgumentNullException("Lot cannot be found");

        if (lot.Auction?.StatusId != (int)AuctionStatusId.Created)
        {
            throw new ArgumentException("Cannot edit lot of started auction");
        }

        var categories = request.Categories?.Select(_unitofWork.Repository.GetById<Category>)
            .Select(t => t.Result)
            .ToHashSet();

        lot.Title = request.Title ?? lot.Title;
        lot.Description = request.Description ?? lot.Description;
        lot.InitialPrice = request.InitialPrice ?? lot.InitialPrice;
        lot.Categories = categories ?? lot.Categories;

        await _unitofWork.SaveChanges();

        var lotDto = LotDto.FromLot(lot);

        return lotDto;
    }
}
