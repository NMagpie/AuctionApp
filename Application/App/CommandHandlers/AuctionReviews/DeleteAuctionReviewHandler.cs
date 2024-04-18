using Application.Abstractions;
using Application.App.Commands.AuctionReviews;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.AuctionReviews;
public class DeleteAuctionReviewHandler : IRequestHandler<DeleteAuctionReviewCommand, AuctionReviewDto>
{
    private readonly IUnitOfWork _unitofWork;

    public DeleteAuctionReviewHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
    }

    public async Task<AuctionReviewDto> Handle(DeleteAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        var auctionReview = await _unitofWork.Repository.Remove<AuctionReview>(request.Id);

        await _unitofWork.SaveChanges();

        var auctionReviewDto = AuctionReviewDto.FromAuctionReview(auctionReview);

        return auctionReviewDto;
    }
}
