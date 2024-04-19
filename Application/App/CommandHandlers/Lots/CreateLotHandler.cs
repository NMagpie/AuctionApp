using Application.Abstractions;
using Application.App.Commands.Lots;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using EntityFramework.Domain.Models;
using FluentValidation.Validators;
using MediatR;
using System.Reflection.Metadata.Ecma335;

namespace Application.App.CommandHandlers.Lots;
public class CreateLotHandler : IRequestHandler<CreateLotCommand, LotDto>
{

    private readonly IUnitOfWork _unitofWork;

    private readonly CreateLotCommandValidator _validator;

    public CreateLotHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
        _validator = new CreateLotCommandValidator();
    }

    public async Task<LotDto> Handle(CreateLotCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var categories = request.Categories
            .Select(_unitofWork.Repository.GetById<Category>)
            .Select(t => t.Result)
            .ToHashSet();

        var lot = new Lot()
        {
            Title = request.Title,
            Description = request.Description ?? "",
            InitialPrice = request.InitialPrice,
            Categories = categories
        };

        if (request.AuctionId != null)
        {
            var auction = await _unitofWork.Repository.GetById<Auction>(request.AuctionId.Value)
                ?? throw new ArgumentNullException("Auciton cannot be found");

            lot.Auction = auction;

            lot.LotOrder = auction.Lots!.Count;
        }

        await _unitofWork.Repository.Add(lot);

        await _unitofWork.SaveChanges();

        var lotDto = LotDto.FromLot(lot);

        return lotDto;
    }
}
